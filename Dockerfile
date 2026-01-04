#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

RUN apt-get update && \
    apt-get install -yq tzdata && \
    ln -fs /usr/share/zoneinfo/America/Sao_Paulo /etc/localtime && \
    dpkg-reconfigure -f noninteractive tzdata

COPY *.sln .
COPY *.props .

COPY src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done

RUN dotnet restore
COPY . .
WORKDIR "/src/src/Notredame.Api"
RUN dotnet build "Notredame.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Notredame.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notredame.Api.dll"]