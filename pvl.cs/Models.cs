using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PVL 
{
    public class Error
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }

    public class Response
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
        
    }

    public class ArrayResponse<T> : Response
    {
        [JsonProperty("result")]
        public T[] Result { get; set; }
    }

    public class DictionaryResponse<T> : Response
    {
        [JsonProperty("result")]
        public Dictionary<string, T> Result { get; set; }
    }

    public class Station
    {
        [JsonProperty("shortcode")]
        public string ShortCode { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("genre")]
        public string Genre { get; set; }
        [JsonProperty("image_url")]
        public string ImageURL { get; set; }
        [JsonProperty("stream_url")]
        public string StreamURL { get; set; }
    }

    public class NowPlayingInfo
    {
        [JsonProperty("current_song")]
        public Song CurrentSong { get; set; }
        [JsonProperty("song_history")]
        public Song[] SongHistory { get; set; }
    }

    public class Song
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("artist")]
        public string Artist { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("rating")]
        public VotingRecord Rating { get; set; }
    }

    public class VotingRecord
    {
        [JsonProperty("current")]
        public int Current { get; set; }
        [JsonProperty("unique")]
        public int Unique { get; set; }
        [JsonProperty("total")]
        public int Total { get; set; }
    }
}