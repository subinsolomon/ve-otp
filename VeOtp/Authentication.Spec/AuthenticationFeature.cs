using FluentAssertions;
using Smocks;
using System;
using System.Collections.Generic;
using System.Linq;
using Ve.Otp.Authentication;
using Xbehave;

namespace Ve.Otp.Authenticator.Spec
{
    public class AuthenticationFeature
    {
        [Scenario]
        public void Generation(string userId, Generator generator, string otp)
        {
            "Given a User ID"
                .f(() => { userId = "thomas_michael_wallace13"; });
            "And a generator"
                .f(() => { generator = new Generator(); });
            "When I request a OTP"
                .f(() => { otp = generator.GenerateUserCurrentOtpFromId(userId); });
            "Then I should be given a short, typable, password."
                .f(() => { otp.Should().MatchRegex(@"^[\w+\\]{6}$"); });
        }

        [Scenario]
        public void Unqiueness(List<string> userIds, Generator generator, IEnumerable<string> otps)
        {
            "Given a selection of User ID"
                .f(() => { userIds = new List<string> { "alexander", "alexandria", "123-456" }; });
            "And a generator"
                .f(() => { generator = new Generator(); });
            "When I generate a series of OTPs"
                .f(() => { otps = userIds.Select(u => generator.GenerateUserCurrentOtpFromId(u)); });
            "They should be unique for each user."
                .f(() => { otps.Should().OnlyHaveUniqueItems(); });
        }

        [Scenario]
        public void Validation(string userId, string otp, Validator validator, bool isValid)
        {
            "Give a User ID"
                .f(() => { userId = "tom123"; });
            "And a generated OTP for that ID"
                .f(() => { otp = (new Generator()).GenerateUserCurrentOtpFromId(userId); });
            "And a OTP validator"
                .f(() => { validator = new Validator(); });
            "When I verify my OTP"
                .f(() => { isValid = validator.ValidateUserFromIdUsingOtp(userId, otp); });
            "It should be valid."
                .f(() => { isValid.Should().BeTrue(); });
        }

        [Scenario]
        [Example(0, true)]
        [Example(30, true)]
        [Example(60, false)]
        [Example(-35, false)]
        public void ValidationWithinTime(int secondsAgo, bool shouldBeValid, string userId, string otp, Validator validator, bool isValid)
        {
            "Given a User ID"
                .f(() => { userId = "tom123"; });
            $"And a OTP generated {secondsAgo} seconds ago"
                .f(() => {
                    Smock.Run(context =>
                    {
                        /*
                         * ToDo: Switch to Fakes with VS2015 Enterprise.
                         * 
                         * Smock's context switching breaks configuration; therefore Generator needs a 
                         * default secret key that must match app.config in this test. 
                         */
                        var now = DateTime.UtcNow;
                        context.Setup(() => DateTime.UtcNow).Returns(now.AddSeconds(-secondsAgo));
                        otp = (new Generator()).GenerateUserCurrentOtpFromId(userId);
                        context.Setup(() => DateTime.UtcNow).Returns(now);
                    });
                });
            "And a OTP validator"
                .f(() => { validator = new Validator(); });
            "When I verify my OTP"
                .f(() => { isValid = validator.ValidateUserFromIdUsingOtp(userId, otp); });

            string validationWording = shouldBeValid ? "valid" : "invalid";
            $"It should be {validationWording}."
                .f(() => { isValid.Should().Be(shouldBeValid); });
        }
    }
}
