using System;
using System.Threading;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Infrastructure.Commons.Constants;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;

namespace Compass.Security.Infrastructure.Services.Externals.Google
{
    public class FirebaseStorageService : IStorageService
    {
        public async Task<string> Upload(IFormFile file)
        {
            var auth = await new FirebaseAuthProvider(new FirebaseConfig(ConfigurationConstant.FirebaseStorageApiKey))
                .SignInWithEmailAndPasswordAsync(ConfigurationConstant.FirebaseStorageUsr, ConfigurationConstant.FirebaseStoragePwd);

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(ConfigurationConstant.FirebaseStorageBucket, new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken),
                    ThrowOnCancel = true,
                })
                .Child(StorageConstant.RouteTemplate)
                .Child(file.FileName)
                .PutAsync(file.OpenReadStream(), cancellation.Token);

            try
            {
                return await task;
            }
            catch (Exception e)
            {
                Console.WriteLine("FirebaseStorageClient : Upload - {0}", e);
            }

            return null;
        }
    }
}