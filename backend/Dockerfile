FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["/TaskManagement/TaskManagement.csproj", "TaskManagement/"]

RUN dotnet restore "TaskManagement/TaskManagement.csproj"

COPY . .

WORKDIR "/src/TaskManagement"
RUN dotnet build "TaskManagement.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "TaskManagement.csproj" -c Release -o /app/publish

FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManagement.dll"]