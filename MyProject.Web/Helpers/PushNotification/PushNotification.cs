using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MyProject.Web.Helpers.FCMNotifications;

namespace MyProject.Web.Helpers.PushNotification
{
    public class PushNotification
    {
        
        private static Uri FireBasePushNotificationsURL = new Uri("https://fcm.googleapis.com/fcm/send");
        private static string ServerKey = "AAAAEgbtR40:APA91bH8rdbUI7Qv7gUCIaUDtJRBINpMwRtm79oW3HzjQLtV1B54YsR7jyc9kC9doI5h7ljQLrZFppHxeMAVf6gHKm_G2v9McTaF8MWvSfaV0zulC9Mcq3VENt_N_ROrVAgnh6VJBHiq";
        private static string ServerKeyForVendor = "AAAAAWCeM2I:APA91bFZYhAXm8mPOwJ_GG8q0VVAiEO8f6-fRv4XYg-tf0JvEcOWqaIotOReOggR2zXM4ElB32mgI7o1kMEiT1sA2YI3Ppl8YIvCqtb3yPT8ZcuJQs1Uh39MCvYUQFZpnflwakirAULR";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        public static async Task<bool> SendPushNotification(string[] deviceTokens, string title, string body, object data,bool forCustomer = true)
        {
            bool sent = false;

            if (deviceTokens.Count() > 0)
            {
                //Object creation

                var messageInformation = new Message()
                {
                    notification = new Notification()
                    {
                        title = title,
                        body = body,
						sound = "default",
					},
                    data = data,
					sound = "default",
					registration_ids = deviceTokens
                };

                //Object to JSON STRUCTURE => using Newtonsoft.Json;
                string jsonMessage = JsonConvert.SerializeObject(messageInformation);

                /*
                 ------ JSON STRUCTURE ------
                 {
                    notification: {
                                    title: "",
                                    text: ""
                                    },
                    data: {
                            action: "Play",
                            playerId: 5
                            },
                    registration_ids = ["id1", "id2"]
                 }
                 ------ JSON STRUCTURE ------
                 */

                //Create request to Firebase API
                var request = new HttpRequestMessage(HttpMethod.Post, FireBasePushNotificationsURL);
                if (forCustomer)
                {
                    request.Headers.TryAddWithoutValidation("Authorization", "key=" + ServerKey);
                }

                else
                {
                    request.Headers.TryAddWithoutValidation("Authorization", "key=" + ServerKeyForVendor);
                }
                
                request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                HttpResponseMessage result;
                using (var client = new HttpClient())
                {
                    result = await client.SendAsync(request);
                    sent = sent && result.IsSuccessStatusCode;
                }
            }

            return sent;
        }
    }
}