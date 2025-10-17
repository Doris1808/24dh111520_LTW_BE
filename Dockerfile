# Sử dụng image chính thức của .NET 8 (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Image để build project (SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy file .csproj và restore dependencies
COPY ["24dh111520_LTW.csproj", "./"]
RUN dotnet restore "./24dh111520_LTW.csproj"

# Copy toàn bộ source code vào container
COPY . .

# Build và publish ra thư mục /app/publish
RUN dotnet publish "24dh111520_LTW.csproj" -c Release -o /app/publish

# Stage cuối: dùng runtime nhẹ để chạy app
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "24dh111520_LTW.dll"]

