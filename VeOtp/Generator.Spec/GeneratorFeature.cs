using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xbehave;

namespace Ve.Otp.Generator.Spec
{
    public class GeneratorFeature
    {
        [Scenario]
        public void Generation(string userId, OtpGenerator generator, string otp)
        {
            "Given a User ID"
                .f(() => { userId = "thomas_michael_wallace13"; });
            "And a generator"
                .f(() => { generator = new OtpGenerator(); });
            "When I request a OTP"
                .f(() => { otp = generator.generate(userId); });
            "Then I should be given a short, typable, password."
                .f(() => { otp.Should().MatchRegex(@"^[\w+\\]{6}$"); });
        }

        [Scenario]
        public void Unqiueness(List<string> userIds, OtpGenerator generator, IEnumerable<string> otps)
        {
            "Given a selection of User ID"
                .f(() => { userIds = new List<string> { "alexander", "alexandria", "123-456" }; });
            "And a generator"
                .f(() => { generator = new OtpGenerator(); });
            "When I generate a series of OTPs"
                .f(() => { otps = userIds.Select(generator.generate); });
            "They should be unique for each user."
                .f(() => { otps.Should().OnlyHaveUniqueItems(); });
        }

        [Scenario]
        public void Validation(string userId, string otp, OtpValidator validator, bool isValid)
        {
            "Give a User ID"
                .f(() => { userId = "tom123"; });
            "And a generated OTP for that ID"
                .f(() => { otp = (new OtpGenerator()).generate(userId); });
            "And a OTP validator"
                .f(() => { validator = new OtpValidator(); });
            "When I verify my OTP"
                .f(() => { isValid = validator.validateUserIdWithOtp(userId, otp); });
            "It should be valid."
                .f(() => { isValid.Should().BeTrue(); });
        }

        [Scenario]
        public void ValidationWithinTime()
        {
            "Given a User ID"
                .f(() => { });
            "And a OTP generated 30 seconds ago"
                .f(() => { });
            "When I verify my OTP"
                .f(() => { });
            "It should be valid."
                .f(() => { throw new NotImplementedException(); });
        }

        [Scenario]
        public void ValidationOutsideOfTime()
        {
            "Given a User ID"
                .f(() => { });
            "And a OTP generated outside of 30 seconds ago"
                .f(() => { });
            "When I verify my OTP"
                .f(() => { });
            "It should be invalid."
                .f(() => { throw new NotImplementedException(); });
        }

    }
}
