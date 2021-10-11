namespace Compass.Security.Application.Commons.Constants
{
    public static class ResponseConstant
    {
        public const string MessageSuccess = "The operation was successful";
        
        public const string MessageSuccessOpt = "We have sent the double factor code for your login, please verify your email";
        
        public const string MessageConfirm = "We have sent a notification for the activation of your account, check your email.";

        public const string MessageSuccessConfirm = "Your account was successfully activated, you can now log in.";

        public const string MessageChangePassword = "We have sent the email instructions to follow and recover the password, please verify your email.";

        public const string MessageSuccessPassword = "Your password was updated successfully, please log in with your new password.";

        public const string MessageLockedAccount = "It seems that you have exceeded the maximum number of attempts, please try again later.";
        
        public const string MessageSignInFail = "Incorrect email or password, please check and try again.";

        public const string MessageTwoFactorFail = "The code is incorrect, please check, or generate a new authentication code.";

        public const string MessageTwoFactorError = "Wrong token, please try again.";

        public const string MessageMaxNotification = "You have exceeded the maximum daily notification submission, please try again later.";
        
        public const string MessageEmpty = "No information was found, with the parameters sent";
        
        public const string MessageFail = "An error occurred in the process";

        public const string MessageErrorProvider = "External signin provider error";
    }
}