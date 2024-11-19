using System.Text.Json.Serialization;
using System.ComponentModel;
using System;
using System.Diagnostics;

namespace StudioGhibliApp
{

public class Movie
{

        private bool _isSelected;
        private string _title;

        //  Debug.WriteLine($"Movie clicked: {clickedMovie.Title}, Selected: {clickedMovie.IsSelected}");

        [JsonPropertyName("title")]
        public string Title
        {
            get { return _title; }
            set
            {
                // Debug print when the setter is called
                Debug.WriteLine(value);

                // Optionally, set the value to the backing field
                _title = value;
            }
        }

        [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("url")]
     public string Url { get; set; }

    [JsonPropertyName("image")] // Only if image is present; otherwise, remove this
    public string Image { get; set; }

    // Add other properties if necessary

      public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

        public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }
    
}

}