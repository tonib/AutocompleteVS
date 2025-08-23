# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

AutocompleteVS is a Visual Studio 2019/2022 extension that provides AI-powered code autocompletion using multiple LLM providers (Ollama, OpenAI, and custom servers). The extension integrates directly into the Visual Studio text editor to provide inline code suggestions.

## Architecture

The solution contains 4 main projects:

### AutocompleteVs (Main Extension)
- **Target Framework**: .NET Framework 4.8
- **Type**: Visual Studio Extension Package (VSIX)
- **Entry Points**: 
  - `AutocompleteVsPackage.cs` - Main VS package registration and initialization
  - `ViewAutocompleteHandler.cs` - Handles text view events and suggestion display
  - `VsTextViewListener.cs` - MEF component that attaches handlers to text views

**Key Components:**
- **Config/**: Model configuration system supporting multiple AI providers
  - `Settings.cs` - VS Tools > Options page configuration
  - `IModelConfig.cs`, `OllamaModelConfig.cs`, `OpenAIModelConfig.cs` - Provider-specific configs
- **SuggestionGeneration/**: Core autocompletion logic
  - `AutocompletionsGenerator.cs` - Main orchestrator for generating suggestions  
  - `Generators/` - Provider-specific implementations (Ollama, OpenAI, CustomServer)
  - `Prompt.cs` - Context building and prompt formatting
- **Extensions/**: Text editor integration utilities
- **LIneTransforms/**: Multi-line completion rendering system using VS editor transforms

### AutocompleteVs.Client  
- **Target Framework**: .NET Standard 2.0
- **Purpose**: Shared client library for communicating with inference servers
- **Key Files**: `InferenceClient.cs`, `IInferenceHub.cs` (SignalR hub interface)

### AutoocompleteVs.Server
- **Target Framework**: .NET 8.0  
- **Type**: ASP.NET Core Web API with SignalR
- **Purpose**: Custom inference server using LlamaSharp for local model execution
- **Key Components**:
  - `Hubs/InferenceHub.cs` - SignalR hub for real-time inference
  - `Models/` - Beam search and batched execution logic

### LlamaSharpServer  
- **Target Framework**: .NET 8.0
- **Purpose**: Test/example client for the custom server

## Development Commands

### Build
```bash
# Build entire solution
msbuild AutocompleteVs.sln /p:Configuration=Release

# Build specific project  
msbuild AutocompleteVs/AutocompleteVs.csproj /p:Configuration=Debug

# Build and package VSIX extension
msbuild AutocompleteVs/AutocompleteVs.csproj /p:Configuration=Release /p:DeployExtension=false
```

### Running/Testing
```bash
# Run the ASP.NET Core server (for custom server testing)
cd AutoocompleteVs.Server
dotnet run

# Run test client
cd LlamaSharpServer  
dotnet run
```

### Visual Studio Extension Development
- The main project (`AutocompleteVs`) can be debugged by pressing F5, which launches a new VS instance with the extension loaded
- Extension settings are available in VS under Tools > Options > AutocompleteVs
- The extension uses `/rootsuffix Exp` for isolated testing

## Key Integration Points

### Text Editor Integration
- Uses Visual Studio's MEF (Managed Extensibility Framework) for component discovery
- Integrates with `ITextView` and `IAdornmentLayer` for suggestion rendering  
- Implements `IKeyFilter` for keyboard handling (Tab to accept suggestions)

### Multi-Provider Support
The suggestion generation system supports:
- **Ollama**: Local model execution via OllamaSharp library
- **OpenAI**: Cloud-based completion via OpenAI API
- **Custom Server**: Local LlamaSharp-based server with advanced features like beam search

### Configuration System
- Settings stored in VS registry via `DialogPage` base class
- Multiple model configurations can be stored and switched between
- Provider-specific parameters (temperature, top-p, max tokens, etc.)

## Important Technical Notes

- **Assembly Loading**: Uses custom DLL references in `_References/` folder due to VS extension assembly loading constraints
- **UI Threading**: Suggestion display must be marshaled to UI thread using `IVsUIShell`
- **Performance**: Implements cancellation tokens and timeouts to prevent blocking the editor
- **Multi-line Suggestions**: Uses VS text transforms for rendering multi-line completions as "ghost text"

## Known Issues/TODO Items
See `AutocompleteVs/TODO.txt` for detailed development todos and known issues including:
- Visual alignment issues with inline suggestions
- Conflicts with built-in VS 2022 suggestions  
- SignalR compatibility challenges in VS 2019
- Performance optimizations needed for large models