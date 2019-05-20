FROM microsoft/aspnetcore

COPY ./publish /publish
WORKDIR /publish
EXPOSE 5000/tcp
ENTRYPOINT ["dotnet", "Raziel.Ork.dll"]