namespace Capstone_Banking.CommonFunction
{
    public class GenerateOtp
    {
        public static double GenerateOtpToEmail()
        {
            Random random = new Random();
            return random.Next(100000, 999999); // 6-digit OTP
        }
    }
}
