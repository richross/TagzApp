FROM mcr.microsoft.com/dotnet/sdk:8.0.101-bookworm-slim-amd64

#this will not run in the aspnet image, so using the sdk image to do the ef bits.
RUN dotnet tool install --global dotnet-ef

# Install locales package
# RUN apt-get clean && apt-get update && apt-get install -y locales
RUN apt-get clean && apt-get update && apt-get install -y locales && \
    sed -i -e 's/# en_US.UTF-8 UTF-8/en_US.UTF-8 UTF-8/' /etc/locale.gen && \
    dpkg-reconfigure --frontend=noninteractive locales && \
    update-locale LANG=en_US.UTF-8

# Generate the locale
RUN locale-gen en_US.UTF-8

# Set the locale environment variables
ENV LANG=en_US.UTF-8  
ENV LANGUAGE=en_US:en  
ENV LC_ALL=en_US.UTF-8 

# [Optional] Uncomment this section to install additional OS packages.
# RUN apt-get update && export DEBIAN_FRONTEND=noninteractive \
#     && apt-get -y install --no-install-recommends <your-package-list-here>

# [Optional] Uncomment this line to install global node packages.
# RUN su vscode -c "source /usr/local/share/nvm/nvm.sh && npm install -g <your-package-here>" 2>&1