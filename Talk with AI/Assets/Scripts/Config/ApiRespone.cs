using System;

namespace Config
{
    [Serializable]
    public class Message
    {
        public string role;
        public string content;
    }
    
    [Serializable]
    public class ChatRequest
    {
        public string model;
        public bool stream;
        public Message[] messages;
        public int max_tokens;
    }
    //
    // [Serializable]
    // public class Choice
    // {
    //     public int index;
    //     public Message message;
    // }
    //
    // [Serializable]
    // public class ChatResponse
    // {
    //     public Choice[] choices;
    // }
    //
    
    [Serializable]
    public class ChunkResponse
    {
        public string id;
        public string @object;
        public long created;
        public string model;
        public Choice[] choices;
    }

    [Serializable]
    public class Choice
    {
        public int index;
        public Delta delta;
        public string finish_reason;
    }

    [Serializable]
    public class Delta
    {
        public string role;    // chỉ có ở chunk đầu tiên
        public string content; // text mới generate ra
    }
}