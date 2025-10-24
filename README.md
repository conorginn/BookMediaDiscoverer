# Book & Media Discoverer

A Blazor WebAssembly application that allows users to discover books and movies by integrating with third party web services.

## 🚀 Features

- **Book Search**: Search for books using the Google Books API
- **Movie Search**: Discover movies using the OMDb API
- **Integrated UI**: Tab-based interface for seamless media discovery
- **Real time Search**: Instant results as you type

## 🛠️ Technologies Used

- **Frontend**: Blazor WebAssembly
- **Language**: C#
- **Framework**: .NET 8.0
- **APIs**: 
  - Google Books API
  - OMDb API (Open Movie Database)
- **Styling**: CSS3 with Grid and Flexbox

## 📋 Prerequisites

- Visual Studio 2022
- .NET 6.0 SDK or later
- Git

## 🏃‍♂️ Running the Project

1. **Clone the repository**
   ```bash
   git clone https://github.com/conorginn/BookMediaDiscoverer.git
   cd BookMediaDiscoverer
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the application**
   Ctrl + F5

## 🔧 API Configuration

### Google Books API
- No API key required for basic usage
- Free tier with generous limits

### OMDb API
1. Get a free API key from [OMDb API](http://www.omdbapi.com/apikey.aspx)
2. Replace the demo key in `Services/OMDbService.cs`

## 🗂️ Project Structure

```
BookMediaDiscoverer/
├── Models/          # Data models (Book, Movie, etc.)
├── Services/        # API service implementations
├── Pages/           # Blazor components
├── wwwroot/         # Static files and CSS
└── Program.cs       # Application entry point
```

## 🌐 API References

### Google Books API
- **Documentation**: [Google Books API Docs](https://developers.google.com/books/docs/v1/using)
- **Base URL**: `https://www.googleapis.com/books/v1/volumes`
- **Rate Limits**: Free usage with quota
- **Authentication**: None required for basic search

### OMDb API
- **Documentation**: [OMDb API Docs](http://www.omdbapi.com/)
- **Base URL**: `http://www.omdbapi.com/`
- **Rate Limits**: 1000 requests per day (free tier)
- **Authentication**: API key required

## 📚 Third-Party Tutorials & References

### Blazor WebAssembly
- **Microsoft Documentation**: [Blazor WebAssembly](https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-6.0)
- **Tutorial**: [Build your first Blazor app](https://docs.microsoft.com/en-us/learn/modules/build-blazor-webassembly-app-first/)

### HTTP Client in Blazor
- **Guide**: [Call a web API from ASP.NET Core Blazor](https://docs.microsoft.com/en-us/aspnet/core/blazor/call-web-api?view=aspnetcore-6.0)
- **Reference**: [System.Net.Http.Json](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.json?view=net-6.0)

### JSON Serialization
- **Documentation**: [System.Text.Json](https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-overview)
- **Guide**: [How to serialize and deserialize JSON in .NET](https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to)

### CSS Grid & Flexbox
- **Guide**: [CSS Grid Layout](https://developer.mozilla.org/en-US/docs/Web/CSS/CSS_Grid_Layout)
- **Tutorial**: [A Complete Guide to Flexbox](https://css-tricks.com/snippets/css/a-guide-to-flexbox/)

## 🔗 Useful Resources

- [Blazor University](https://blazor-university.com/) - Comprehensive Blazor learning resource
- [Awesome Blazor](https://github.com/AdrienTorris/awesome-blazor) - Curated list of Blazor resources
- [Public APIs](https://github.com/public-apis/public-apis) - Collection of free APIs for development

## 🧪 Testing

The application includes:
- Error handling for API failures
- Loading states during searches
- Empty state messages
- Input validation

## 👥 Development Workflow

This project was developed using feature branches:
- `master` - Stable integrated application
- `feature/books-service` - Google Books API implementation
- `feature/movies-service` - OMDb API implementation

## 📄 License

This project is for educational purposes as part of the DkIT Service Oriented Architecture assessment.

## 🙏 Acknowledgments

- **Google** for providing Books API
- **OMDb** for movie database API
- **Microsoft** for Blazor framework and documentation
