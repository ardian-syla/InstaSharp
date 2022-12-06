static void FollowUser(string id, string sessionID)
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://i.instagram.com/api/v1/web/friendships/{id}/follow/");
                request.Method = "POST";
                request.Headers.Add("authority", "i.instagram.com");
                request.Accept = "*/*";
                request.Headers.Add("accept-language", "en-US,en;q=0.9");
                request.Headers.Add("cookie", $"sessionid={sessionID};");
                request.Headers.Add("origin", "https://www.instagram.com");
                request.Referer = "https://www.instagram.com/";
                request.ContentType = " application/x-www-form-urlencoded";
                request.ContentLength = 0;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36";
                request.Headers.Add("x-asbd-id", "198387");
                request.Headers.Add("x-csrftoken", "missing");
                request.Headers.Add("x-ig-app-id", "936619743392459");
                request.Headers.Add("x-ig-www-claim", "hmac.AR2pitRBXWeGMybAC8XIej3q4FpvQjB27u7pmysXYjSkIbBy");
                request.Headers.Add("x-instagram-ajax", "1006430745");


                HttpWebResponse Resp;
                try
                {
                    Resp = (HttpWebResponse)request.GetResponse();
                }
                catch (WebException ex)
                {
                    Resp = (HttpWebResponse)ex.Response;
                }
                int respCode = Convert.ToInt32(Resp.StatusCode);
                if (respCode == 404)
                {
                    MessageBox.Show("Something went wrong, User doesnt exist or check your connection!", "iFollow", 5000);
                }
                else if (respCode == 200)
                {
                    StreamReader StreamReader = new StreamReader(Resp.GetResponseStream());
                    string Response = StreamReader.ReadToEnd();
                    if (Response.Contains("\"result\":\"following\",\"status\":\"ok\""))
                    {
                        MessageBox.Show("Followed Successfully!", "InstaSharp");
                    }
                    else if (Response.Contains("spam"))
                    {
                        MessageBox.Show("Something went wrong, spam!", "InstaSharp");
                    }
                    else
                    {
                        MessageBox.Show("Something went wrong!", "InstaSharp");
                    }
                }
                else if (respCode == 400)
                {
                    MessageBox.Show("Something went wrong, Bad Request [ Status Code: 400 ]!", "InstaSharp");
                }
                else
                {
                    MessageBox.Show("Something went wrong please try again", "InstaSharp");
                }
            }
