# Stage 1: Build with .NET SDK and AOT publish
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Create runtime image
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS final
# Add globalization support and configure for Workstation garbage collection
# See https://github.com/dotnet/dotnet-docker/blob/main/samples/enable-globalization.md#alpine-images
# and https://learn.microsoft.com/en-us/dotnet/core/runtime-config/garbage-collector
ENV \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true \
    DOTNET_gcServer=0 \ 
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8

RUN apk add --no-cache \
    icu-data-full \
    icu-libs \
    tzdata

# Run the service
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["./MinimalApi"]