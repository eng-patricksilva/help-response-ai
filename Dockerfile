FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS production-stage

ARG ENTRYPOINT_DLL
ENV ENTRYPOINT_DLL=${ENTRYPOINT_DLL}


ENV ASPNETCORE_URLS=http://+:8080 \
    COMPlus_EnableDiagnostics=1 \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false   

EXPOSE 8080
WORKDIR /app
COPY . .


RUN apk add --no-cache tzdata icu-libs icu-data-full wget && \
    ln -sf /usr/share/zoneinfo/America/Sao_Paulo /etc/localtime && \
    echo "America/Sao_Paulo" > /etc/timezone

ENV LANG=pt_BR.UTF-8
ENV LANGUAGE=pt_BR:en
ENV LC_ALL=pt_BR.UTF-8

# Allow TLSv1.2 ou v1 
RUN sed -i 's/providers = provider_sect/providers = provider_sect\n\
  ssl_conf = ssl_sect\n\
  \n\
  [ssl_sect]\n\
  system_default = system_default_sect\n\
  \n\
  [system_default_sect]\n\
  Options = UnsafeLegacyRenegotiation/' /etc/ssl/openssl.cnf

HEALTHCHECK --interval=1m --timeout=3s \
  CMD wget --no-verbose --tries=1 --spider http://localhost:8080/health-check || exit 1

USER $APP_UID
ENTRYPOINT sh -c "dotnet ${ENTRYPOINT_DLL}"