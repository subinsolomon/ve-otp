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
                .f(() => { userId = "tomw"; });
            "And a generator"
                .f(() => { generator = new OtpGenerator(); });
            "When I request a OTP"
                .f(() => { otp = generator.generate(userId); });
            "Then I should be given a short, typable, password."
                .f(() => { otp.Should().MatchRegex(@"^\w{6}$"); });
        }

        [Scenario]
        public void Unqiueness(List<string> userIds, OtpGenerator generator, IEnumerable<string> otps)
        {
            "Given a selection of User ID"
                .f(() => { userIds = new List<string> { "alexande", "alexandr", "1234" }; });
                // ToDo: Support non-4 letter long names.
            "And a generator"
                .f(() => { generator = new OtpGenerator(); });
            "When I generate a series of OTPs"
                .f(() => { otps = userIds.Select(generator.generate); });
            "They should be unique for each user."
                .f(() => { otps.Should().OnlyHaveUniqueItems(); });
        }

        [Scenario]
        public void Validation()
        {
            "Give a User ID"
                .f(() => { });
            "And a generated OTP for that ID"
                .f(() => { });
            "When I verify my OTP"
                .f(() => { });
            "It should be valid."
                .f(() => { throw new NotImplementedException(); });
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
