using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace BuildingManagementWsClient
{
    public class ApiClient<T>
    {
        HttpClient httpClient = VecinoHttpClient.Instance;
        UriBuilder uriBuilder = new UriBuilder();

        public string Scheme
        {
            set
            {
                this.uriBuilder.Scheme = value;
            }
        }
        public string Host
        {
            set
            {
                this.uriBuilder.Host = value;
            }
        }
        public int Port
        {
            set
            {
                this.uriBuilder.Port = value;
            }
        }
        public string Path
        {
            set
            {
                this.uriBuilder.Path = value;
            }
        }
        public void AddParameter(string key,string value)
        {
            if (this.uriBuilder.Query == string.Empty)
                this.uriBuilder.Query += "?";
            else
                this.uriBuilder.Query += "&";
            this.uriBuilder.Query += $"{key}={value}";
        }
        public async Task<T> GetAsync() // getting data from webservices 
        {
            using (HttpRequestMessage httpRequest = new HttpRequestMessage())
            {
                httpRequest.Method = HttpMethod.Get;
                httpRequest.RequestUri = this.uriBuilder.Uri;
                using (HttpResponseMessage httpResponse = await this.httpClient.SendAsync(httpRequest))
                {
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        string result = await httpResponse.Content.ReadAsStringAsync();
                        JsonSerializerOptions options = new JsonSerializerOptions();
                        options.PropertyNameCaseInsensitive = true;
                        T model = JsonSerializer.Deserialize<T>(result, options);
                        return model;

                    }
                    
                    return default(T);

                }
            }
        }
        public async Task<bool> PostAsync(T model)
        {
            using (HttpRequestMessage httpRequest = new HttpRequestMessage())
            {
                httpRequest.Method = HttpMethod.Post;
                httpRequest.RequestUri = this.uriBuilder.Uri;
                string json = JsonSerializer.Serialize<T>(model);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                httpRequest.Content = content;
                using(HttpResponseMessage httpResponse = await this.httpClient.SendAsync(httpRequest))
                {
                    return httpResponse.IsSuccessStatusCode;
                }
             
            }
        }
       
        public async Task<TResponse> PostAsyncReturn<TRequest,TResponse>(TRequest model)
        {

            using (HttpRequestMessage httpRequest = new HttpRequestMessage())
            {
                httpRequest.Method = HttpMethod.Post;
                httpRequest.RequestUri = this.uriBuilder.Uri;
                string json = JsonSerializer.Serialize<TRequest>(model);
                StringContent content = new StringContent(json,Encoding.UTF8,"application/json");
                httpRequest.Content = content;
                using (HttpResponseMessage httpResponse = await this.httpClient.SendAsync(httpRequest))
                {
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        string result = await httpResponse.Content.ReadAsStringAsync();
                        JsonSerializerOptions options = new JsonSerializerOptions();
                        options.PropertyNameCaseInsensitive = true; 
                        TResponse value = JsonSerializer.Deserialize<TResponse>(result, options);
                        return value;
                    }
                }
                return default(TResponse);

            }
        }
        public async Task<bool> PostAsync(T model, Stream file)
            {
                using (HttpRequestMessage httpRequest = new HttpRequestMessage())
                {
                    httpRequest.Method = HttpMethod.Post;
                    httpRequest.RequestUri = this.uriBuilder.Uri;
                    MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
                    string json = JsonSerializer.Serialize<T>(model);
               
                    StringContent modelContent = new StringContent(json);
                    multipartFormDataContent.Add(modelContent,"model");

                    StreamContent streamContent = new StreamContent(file); // Streamcontent becaues of the file stream
                    multipartFormDataContent.Add(streamContent, "file", "file");

                    httpRequest.Content = multipartFormDataContent;
                    using (HttpResponseMessage httpResponse = await this.httpClient.SendAsync(httpRequest))
                    {
                        return httpResponse.IsSuccessStatusCode;
                    }

                }
            
            }
        public async Task<bool> PostAsync(T model, List<Stream> files)
        {
            using (HttpRequestMessage httpRequest = new HttpRequestMessage())
            {
                httpRequest.Method = HttpMethod.Post;
                httpRequest.RequestUri = this.uriBuilder.Uri;
                MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
                string json = JsonSerializer.Serialize<T>(model);

                StringContent modelContent = new StringContent(json);
                multipartFormDataContent.Add(modelContent, "model");

                 
                foreach (Stream file in files)
                {
                    StreamContent streamContent = new StreamContent(file);
                    multipartFormDataContent.Add(streamContent, "file", "file");
                }
              

                httpRequest.Content = multipartFormDataContent;
                using (HttpResponseMessage httpResponse = await this.httpClient.SendAsync(httpRequest))
                {
                    return httpResponse.IsSuccessStatusCode;
                }

            }

        }
    }
}
