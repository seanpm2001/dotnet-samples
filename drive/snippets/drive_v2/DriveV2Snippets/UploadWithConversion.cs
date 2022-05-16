// Copyright 2022 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// [START drive_upload_with_conversion]

using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2beta;
using Google.Apis.Services;


namespace DriveV2Snippets
{
    // Class to demonstrate Drive's upload with conversion use-case.
    public class UploadWithConversion
    {
        
        /// <summary>
        /// Upload file with conversion.
        /// </summary>
        /// <returns>Inserted file id if successful, null otherwise.</returns>
        public static string DriveUploadWithConversion()
        {
            try
            {
                /* Load pre-authorized user credentials from the environment.
                 TODO(developer) - See https://developers.google.com/identity for
                 guides on implementing OAuth2 for your application. */
                GoogleCredential credential = GoogleCredential.GetApplicationDefault()
                    .CreateScoped(DriveService.Scope.Drive);

                // Create Drive API service.
                var service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Drive API Snippets"
                });
               // Upload file My Report on drive
                var fileMetadata = new Google.Apis.Drive.v2beta.Data.File()
                {
                    Title = "My Report",
                    MimeType = "application/vnd.google-apps.spreadsheet"
                };
                FilesResource.InsertMediaUpload request;
                using (var stream = new FileStream("files/report.csv",
                           FileMode.Open))
                {
                    // Create a new file, with metadata and stream.
                    request = service.Files.Insert(
                        fileMetadata, stream, "text/csv");
                    request.Fields = "id";
                    request.Upload();
                }
                var file = request.ResponseBody;
                // Prints the uploaded file id.
                Console.WriteLine("File ID: " + file.Id);
                
                return file.Id;
            }
            catch (Exception e)
            {
                // TODO(developer) - handle error appropriately
                if (e is AggregateException)
                {
                    Console.WriteLine("Credential Not found");
                }
                else if (e is GoogleApiException)
                {
                    Console.WriteLine(" Failed With an Error {0}",e.Message);
                }
             
                else
                {
                    throw;
                }
            }
            return null;
        }
    }
}
// [END drive_upload_with_conversion]