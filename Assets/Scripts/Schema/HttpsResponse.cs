using System.Collections.Generic;

namespace DataBase {
    [System.Serializable]
    public class HttpsResponse<T> {
        public string error_code;
        public string msg;
        public List<T> content;
        public bool state;
    }
}