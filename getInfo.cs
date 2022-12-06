// On Session we pass the session we extracted from login on the code up
static async void GetInfo(string Session, string user)
            {
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://i.instagram.com/api/v1/users/web_profile_info/?username=" + user);

                request.Headers.Add("authority", "i.instagram.com");
                request.Headers.Add("accept", "*/*");
                request.Headers.Add("accept-language", "en-US,en;q=0.9");
                request.Headers.Add("cookie", $"sessionid={Session};");
                request.Headers.Add("origin", "https://www.instagram.com");
                request.Headers.Add("referer", "https://www.instagram.com/");
                request.Headers.Add("sec-fetch-dest", "empty");
                request.Headers.Add("sec-fetch-mode", "cors");
                request.Headers.Add("sec-fetch-site", "same-site");
                request.Headers.Add("sec-gpc", "1");
                request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36");
                request.Headers.Add("x-asbd-id", "198387");
                request.Headers.Add("x-csrftoken", "e3HyhQe9Pp0edhAnV7A3cOhAh6SnNn6p");
                request.Headers.Add("x-ig-app-id", "936619743392459");
                request.Headers.Add("x-ig-www-claim", "hmac.AR2pitRBXWeGMybAC8XIej3q4FpvQjB27u7pmysXYjSkIbBy");
                request.Headers.Add("x-instagram-ajax", "1006430745");

                HttpResponseMessage response = await client.SendAsync(request);
                int respcode = Convert.ToInt32(response.StatusCode);
                if (respcode == 404)
                {
                    MessageBox.Show("User Not Found!");
                    Application.Exit();
                }
                else if (respcode == 200)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic obj = JsonConvert.DeserializeObject(responseBody);
                    // get id of instagram user
                    string id = obj.data.user.id;
                    // get followers count
                    var followers = obj.data.user.edge_followed_by.count;
                    // get following count
                    var following = obj.data.user.edge_follow.count;
                    // get username of account
                    var username = obj.data.user.username;
                    // get biography
                    var biography = obj.data.user.biography;
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Unknown Error: {responseBody}", "InstaSharp");
                    Thread.Sleep(5000);
                }
            }
