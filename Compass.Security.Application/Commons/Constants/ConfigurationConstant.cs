using System;
using Microsoft.Extensions.Configuration;

namespace Compass.Security.Application.Commons.Constants
{
    public static class ConfigurationConstant
    {
        #region constant
        private const string Development = "Development";
        #endregion
        
        #region General
        public static string ConnectionString { get; set; }

        public const string Avatar = "https://2mingenieria.com.ve/wp-content/uploads/2018/10/kisspng-avatar-user-medicine-surgery-patient-avatar-5acc9f7a7cb983.0104600115233596105109-300x300.jpg";

        public const int UserMaxEmail = 5;

        public const int UserMaxSms = 5;
        #endregion
        
        #region oAuth
        public static string GoogleAuthKey { get; set; }
        public static string GoogleAuthSecret { get; set; }
        #endregion
        
        #region reCatpcha
        public static string GoogleCaptchaUrl { get; set; }
        public static string GoogleCaptchaKey { get; set; }
        public static string GoogleCaptchaSecret { get; set; }
        #endregion

        #region SendInBlue
        public static string SendInBlueApiKey { get; set; }
        #endregion
        
        #region Firebase
        public static string FirebaseStorageApiKey { get; set; }
        public static string FirebaseStorageBucket { get; set; }
        public static string FirebaseStorageUsr { get; set; }
        public static string FirebaseStoragePwd { get; set; }
        #endregion

        #region Templates
        public static string TemplateActivation { get; set; }
        public static string TemplateWelcome { get; set; }
        public static string TemplateReset { get; set; }
        public static string TemplatePassword { get; set; }
        public static string TemplateOtp { get; set; }
        #endregion

        public static void LoadSetting(IConfiguration configuration)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            Console.WriteLine($"Environment : {environment}");

            if (environment is Development)
            {
                ConnectionString = configuration.GetConnectionString("DbConnection");
                SendInBlueApiKey = configuration.GetSection("SendInBlue:ApiKey").Value;
                FirebaseStorageApiKey = configuration.GetSection("Firebase:Storage:ApiKey").Value;
                FirebaseStorageBucket = configuration.GetSection("Firebase:Storage:Bucket").Value;
                FirebaseStorageUsr = configuration.GetSection("Firebase:Storage:Usr").Value;
                FirebaseStoragePwd = configuration.GetSection("Firebase:Storage:Pwd").Value;
                TemplateActivation = configuration.GetSection("Templates:Activation").Value;
                TemplateWelcome = configuration.GetSection("Templates:Welcome").Value;
                TemplateReset = configuration.GetSection("Templates:Reset").Value;
                TemplatePassword = configuration.GetSection("Templates:Password").Value;
                TemplateOtp = configuration.GetSection("Templates:Otp").Value;
                GoogleCaptchaUrl = configuration.GetSection("GoogleCaptcha:Endpoint").Value;
                GoogleCaptchaKey = configuration.GetSection("GoogleCaptcha:Key").Value;
                GoogleCaptchaSecret = configuration.GetSection("GoogleCaptcha:Secret").Value;
                GoogleAuthKey = configuration.GetSection("oAuth:Google:Key").Value;
                GoogleAuthSecret = configuration.GetSection("oAuth:Google:Secret").Value;
            }
            else
            {
                ConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
                SendInBlueApiKey = Environment.GetEnvironmentVariable("SENDINBLUE_APIKEY");
                FirebaseStorageApiKey = Environment.GetEnvironmentVariable("FI_STORAGE_APIKEY");
                FirebaseStorageBucket = Environment.GetEnvironmentVariable("FI_STORAGE_BUCKET");
                FirebaseStorageUsr = Environment.GetEnvironmentVariable("FI_STORAGE_USR");
                FirebaseStoragePwd = Environment.GetEnvironmentVariable("FI_STORAGE_PWD");
                TemplateActivation = Environment.GetEnvironmentVariable("TEMPLATE_ACTIVATION");
                TemplateWelcome = Environment.GetEnvironmentVariable("TEMPLATE_WELCOME");
                TemplateReset = Environment.GetEnvironmentVariable("TEMPLATE_RESET");
                TemplatePassword = Environment.GetEnvironmentVariable("TEMPLATE_PASSWORD");
                TemplateOtp = Environment.GetEnvironmentVariable("TEMPLATE_OTP");
                GoogleCaptchaUrl = Environment.GetEnvironmentVariable("G_CAPTCHA_ENDPOINT");
                GoogleCaptchaKey = Environment.GetEnvironmentVariable("G_CAPTCHA_KEY");
                GoogleCaptchaSecret = Environment.GetEnvironmentVariable("G_CAPTCHA_SECRET");
                GoogleAuthKey = Environment.GetEnvironmentVariable("G_OAUTH_KEY");
                GoogleAuthSecret = Environment.GetEnvironmentVariable("G_OAUTH_SECRET");
            }

            Console.WriteLine($"ConnectionString : {ConnectionString}");
            Console.WriteLine($"SendInBlueApiKey : {SendInBlueApiKey}");
            Console.WriteLine($"FirebaseStorageApiKey : {FirebaseStorageApiKey}");
            Console.WriteLine($"FirebaseStorageBucket : {FirebaseStorageBucket}");
            Console.WriteLine($"FirebaseStorageUsr : {FirebaseStorageUsr}");
            Console.WriteLine($"FirebaseStoragePwd : {FirebaseStoragePwd}");
            Console.WriteLine($"TemplateActivation : {TemplateActivation}");
            Console.WriteLine($"TemplateWelcome : {TemplateWelcome}");
            Console.WriteLine($"TemplateReset: {TemplateReset}");
            Console.WriteLine($"TemplatePassword : {TemplatePassword}");
            Console.WriteLine($"TemplateOtp : {TemplateOtp}");
            Console.WriteLine($"GoogleCaptchaKey : {GoogleCaptchaKey}");
            Console.WriteLine($"GoogleCaptchaSecret : {GoogleCaptchaSecret}");
            Console.WriteLine($"GoogleAuthKey : {GoogleAuthKey}");
            Console.WriteLine($"GoogleAuthSecret : {GoogleAuthSecret}");
        }
    }
}