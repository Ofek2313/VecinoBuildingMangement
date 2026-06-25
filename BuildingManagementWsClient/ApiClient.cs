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

       

        public ApiClient()
        {
            this.uriBuilder.Scheme = "http";
            this.uriBuilder.Host = "localhost";
            this.uriBuilder.Port = 5269;
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

        public async Task<(byte[] bytes,string contentType)> GetFileAsync() // getting data from webservices 
        {
            //this function is used to pass a file as bytes from the webservice to the clients.
            using (HttpRequestMessage httpRequest = new HttpRequestMessage())
            {
                httpRequest.Method = HttpMethod.Get;
                httpRequest.RequestUri = this.uriBuilder.Uri;
                using (HttpResponseMessage httpResponse = await this.httpClient.SendAsync(httpRequest))
                {
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        byte[] result = await httpResponse.Content.ReadAsByteArrayAsync();
                        string contentType = httpResponse.Content.Headers.ContentType.ToString() ?? "image/jpeg"; // the contentype that is  by deafult is image, since it is always used for images in my project
                        return (result,contentType);

                        
                    }

                    return (null,null);

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
       
        public async Task<ApiResponse<TResponse>> PostAsyncReturn<TRequest,TResponse>(TRequest model) //post method that returns a value, since in our web service we return true of false we always use this
        {

            using (HttpRequestMessage httpRequest = new HttpRequestMessage())
            {
                ApiResponse<TResponse> response = new ApiResponse<TResponse>(); 
                httpRequest.Method = HttpMethod.Post;
                httpRequest.RequestUri = this.uriBuilder.Uri;
                string json = JsonSerializer.Serialize<TRequest>(model);
                StringContent content = new StringContent(json,Encoding.UTF8,"application/json");
                httpRequest.Content = content;
                //constructs an http request using the path, scheme, and port, and sends it.
                using (HttpResponseMessage httpResponse = await this.httpClient.SendAsync(httpRequest)) 
                {
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        //if there was a response it reads, and then serializes it into the type of TResponse.
                        
                        string result = await httpResponse.Content.ReadAsStringAsync();
                        //if (typeof(TResponse) == typeof(string))
                        //    return result;
                        if (string.IsNullOrWhiteSpace(result))
                        {
                            response.Success = false;
                            response.Data = default(TResponse);
                            return response;
                        }
                            
                        JsonSerializerOptions options = new JsonSerializerOptions();
                        options.PropertyNameCaseInsensitive = true; 
                        TResponse value = JsonSerializer.Deserialize<TResponse>(result, options);
                        
                        response.Success = true;
                        response.Data = value;
                        return response; // return a success status of the post, and the data which can be a model, or a bool.

                    }
                    else
                    {
                        string error = await httpResponse.Content.ReadAsStringAsync();
                        JsonSerializerOptions options = new JsonSerializerOptions();
                        options.PropertyNameCaseInsensitive = true;
                        ApiError apiError = JsonSerializer.Deserialize<ApiError>(error, options);
                        response.ErrorMessage = apiError.ErrorMessage;
                    }

                }
                response.Success = false;
                response.Data = default(TResponse);
                
                return response;

            }
            
        }
        //an overloaded function used to postasync buit with a file.
        public async Task<ApiResponse<TResponse>> PostAsyncReturn<TRequest, TResponse>(TRequest model,Stream file,string fileName)
        {

            using (HttpRequestMessage httpRequest = new HttpRequestMessage())
            {
                ApiResponse<TResponse> response = new ApiResponse<TResponse>();
                httpRequest.Method = HttpMethod.Post;
                httpRequest.RequestUri = this.uriBuilder.Uri;
                MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
                string json = JsonSerializer.Serialize<TRequest>(model);
                StringContent modelContent = new StringContent(json);
                multipartFormDataContent.Add(modelContent, "model");

                if(file != null && fileName != null)
                {
                    StreamContent streamContent = new StreamContent(file); // Streamcontent becaues of the file stream
                    multipartFormDataContent.Add(streamContent, "file", fileName);
                }
               
                httpRequest.Content = multipartFormDataContent;
                //constructs a mutlipartform includinga model and a file.
                using (HttpResponseMessage httpResponse = await this.httpClient.SendAsync(httpRequest))
                {
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        string result = await httpResponse.Content.ReadAsStringAsync();
                        //if (typeof(TResponse) == typeof(string))
                        //    return result;
                        if (string.IsNullOrWhiteSpace(result))
                        {
                            response.Success = false;
                            response.Data = default(TResponse);
                            return response;
                        }

                        JsonSerializerOptions options = new JsonSerializerOptions();
                        options.PropertyNameCaseInsensitive = true;
                        TResponse value = JsonSerializer.Deserialize<TResponse>(result, options);

                        response.Success = true;
                        response.Data = value;
                        return response;  // return a success status of the post, and the data which can be a model, or a bool.
                    }
                    else
                    {
                        string error = await httpResponse.Content.ReadAsStringAsync();
                        JsonSerializerOptions options = new JsonSerializerOptions();
                        options.PropertyNameCaseInsensitive = true;
                        ApiError apiError = JsonSerializer.Deserialize<ApiError>(error,options);
                        response.ErrorMessage = apiError.ErrorMessage;

                    }
                }
                response.Success = false;
                response.Data = default(TResponse);
                return response;

            }

        }
        public async Task<bool> PostAsync(T model, Stream file,string fileName)
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
                    multipartFormDataContent.Add(streamContent, "file", fileName);

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
