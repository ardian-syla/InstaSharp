# InstaSharp
InstaSharp is a API for Instagram which integrate's with c# language
```
On file's that i've updated are some example's of how to login,
Follow users, scrape their info throught c# code
```
# Login
This code below shows how to login on instagram throught c#
```csharp
/* Option 2 to login to only 1 account
static void LoginWEB2(string Username, string Password)
then we remove foreach and splited username & password,
we modify string Data as example string Data = $"username={Username}&enc_password=#PWD_INSTAGRAM_BROWSER:0:1589682409:{Password}";
*/
static void LoginWEB2()
            {
                try
                {
                // multi login from file
                    foreach (var line in File.ReadLines("Accounts.txt"))
                    {

                        var splitedUsername = line.Split(':')[0].Split(' ')[0];
                        var splitedPassword = line.Split(':')[1].Split(' ')[0];

                        string Data = $"username={splitedUsername}&enc_password=#PWD_INSTAGRAM_BROWSER:0:1589682409:{splitedPassword}";
                        HttpWebRequest WEBREQUEST = (HttpWebRequest)WebRequest.Create("https://www.instagram.com/accounts/login/ajax/");
                        WEBREQUEST.Method = "POST";
                        WEBREQUEST.Host = "www.instagram.com";
                        WEBREQUEST.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36";
                        WEBREQUEST.ContentType = "application/x-www-form-urlencoded";
                        WEBREQUEST.Headers.Add("x-csrftoken", "missing");
                        WEBREQUEST.Headers.Add("accept-language", "en-US,en;q=0.9,ar-SA;q=0.8,ar;q=0.7");
                        WEBREQUEST.Headers.Add("cookie", "mid=YBFqywABAAE0ZL2yz_x6XtohlZPN; csrftoken=YVjsmppWJc6ylI1luYyrMOPTlfIfGXva;");
                        WEBREQUEST.Headers.Add("x-instagram-ajax", "1");
                        WEBREQUEST.Headers.Add("x-requested-with", "XMLHttpRequest");
                        WEBREQUEST.KeepAlive = true;
                        WEBREQUEST.Timeout = 120000;
                        WEBREQUEST.CookieContainer = CookieContainer;
                        WEBREQUEST.ProtocolVersion = HttpVersion.Version11;
                        WEBREQUEST.ServicePoint.UseNagleAlgorithm = false;
                        WEBREQUEST.ServicePoint.Expect100Continue = false;
                        byte[] bytes = Encoding.ASCII.GetBytes(Data);
                        WEBREQUEST.ContentLength = (long)bytes.Length;
                        Stream Stream = WEBREQUEST.GetRequestStream();
                        Stream.Write(bytes, 0, bytes.Length);
                        Stream.Flush();
                        Stream.Close();
                        Stream.Dispose();
                        HttpWebResponse Resp;
                        try
                        {
                            Resp = (HttpWebResponse)WEBREQUEST.GetResponse();
                        }
                        catch (WebException ex)
                        {
                            Resp = (HttpWebResponse)ex.Response;
                        }
                        StreamReader StreamReader = new StreamReader(Resp.GetResponseStream());
                        string Response = StreamReader.ReadToEnd().ToString();
                        bool DoneLogin = Response.Contains("\"authenticated\":true,\"");
                        bool Secure = Response.Contains("checkpoint_required");
                        bool bad = Response.Contains("\"authenticated\":false,\"");
                        bool idk = Response.Contains("errors");
                        bool Spammed = Response.Contains("wait") || Response.Contains("spam");
                        if (DoneLogin)
                        {
                            MessageBox.Show($"Done Logged In... {splitedUsername}", "InstaSharp", 3000);
                            string IDsess = Convert.ToString(Resp.Cookies["sessionid"]);
                            ard = IDsess.Replace("sessionid=", string.Empty);
                            // extracts the sessionID that we will need to manipulate with instagram
                        }
                        else if (Secure)
                        {
                            MessageBox.Show($"Checkpoint required... {splitedUsername}", "InstaSharp");
                        }
                        else if (Spammed)
                        {
                            MessageBox.Show($"Spam on account {splitedUsername}...", "InstaSharp");
                        }
                        else if (bad)
                        {
                            MessageBox.Show($"Incorrect Password on {splitedUsername}...", "InstaSharp");
                        }
                        else if (idk)
                        {
                            MessageBox.Show("Sorry, there was a problem with your request", "InstaSharp");
                    }
                        else
                        {
                            MessageBox.Show($"Error on account {splitedUsername}...", "InstaSharp");
                            MessageBox.Show($"Error: {Response}...", "InstaSharp");
                        }
                        StreamReader.Dispose();
                        StreamReader.Close();
                    }
                    MessageBox.Show("Logged In Successfully");
                    Thread.Sleep(10000);
                }
                catch (Exception)
                {
                }
            }
```
# Get Info 
Code below shows how to get instagram users info

```csharp
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
```
# Follow a user
On code below ill show a example of how to follow a user on instagram
from c#
```csharp
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
```
