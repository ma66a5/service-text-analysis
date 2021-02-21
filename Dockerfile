FROM mcr.microsoft.com/dotnet/sdk AS build-env
WORKDIR /app

# Copy csproj and restore
COPY ./Services.TextAnalysis/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY ./Services.TextAnalysis/ ./
RUN dotnet publish -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Services.TextAnalysis.dll"]