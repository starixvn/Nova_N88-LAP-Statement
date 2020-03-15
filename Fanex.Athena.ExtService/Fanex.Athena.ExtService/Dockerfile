FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Fanex.Athena.ExtService/Fanex.Athena.ExtService.csproj", "Fanex.Athena.ExtService/"]
COPY ["Fanex.Athena.ExtService.BetList/Fanex.Athena.ExtService.BetList.csproj", "Fanex.Athena.ExtService.BetList/"]
COPY ["Fanex.Athena.ExtService.Resources/Fanex.Athena.ExtService.Resources.csproj", "Fanex.Athena.ExtService.Resources/"]
COPY ["Fanex.Athena.ExtService.Common/Fanex.Athena.ExtService.Common.csproj", "Fanex.Athena.ExtService.Common/"]
COPY ["Fanex.Athena.ExtService.Core/Fanex.Athena.ExtService.Core.csproj", "Fanex.Athena.ExtService.Core/"]
RUN dotnet restore "Fanex.Athena.ExtService/Fanex.Athena.ExtService.csproj"
COPY . .
WORKDIR "/src/Fanex.Athena.ExtService"
RUN dotnet build "Fanex.Athena.ExtService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Fanex.Athena.ExtService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Fanex.Athena.ExtService.dll"]