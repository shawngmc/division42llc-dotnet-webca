# division42llc/dotnet-webca
Docker Image: microsoft/aspnetcore-build:latest image with a custom CA web 
application hosted on port 80. This is a web application which hosts a self-signed
Certificate Authority. You can create/re-create the CA, and issue/delete leaf 
certificates at-will.

>PLEASE NOTE: This application is still not fully-functional.

## Overview
The purpose of this project is to make it simple and easy to stand up a new 
Certificate Authority. That is, a system which can dispense x.509/SSL/TLS "certificates".

If you work within a big company, you can likely get certificates (with some 
ceremony) from your security area. If you host internet-facing applications, 
you can automate certificates via [Let's Encrypt](https://www.letsencrypt.org).

However, for many other scenarios, it would be ideal to have your own CA. For example:

 * For simple/quick testing
 * For your home-lab (your router, NAS, Raspberry Pi's, etc.)
 * For smaller companies, for your intranet.
 * For infrastructure-facing uses - like a private CA for Docker Swarm.

The point is, in the year 2017, you should be able to have a simple and easy 
way to have a Certificate Authority, and now, you can!

## Getting Started...



### Running as a daemon

To run this headless, as a daemon, exposing the website on http://localhost:8080, 
and mapping /var/localCA/ in the container, to your $HOME/Desktop/localCA/ 
directory, and limiting the container to use only 40MB of RAM (usually runs ~27MB), run:

#### On Linux or macOS:
```
$ docker run -d -p 8080:80 -v ~/Desktop/localCA/:/var/localCA/ 
	--memory=40m division42llc/dotnet-webca
```
#### On Windows:
```
$ docker run -d -p 8080:80 -v %UserProfile%/Desktop/localCA/:/var/localCA/ 
	--memory=40m division42llc/dotnet-webca
```

### Running interactively
To run this headless, as a daemon, exposing the website on http://localhost:8080, 
and mapping /var/localCA/ in the container, to your $HOME/Desktop/localCA/ directory, 
and limiting the container to use only 40MB of RAM (usually runs ~27MB), run:

#### On Linux or macOS:
```
$ docker run -it -p 8080:80 -v ~/Desktop/localCA/:/var/localCA/ 
	--memory=40m division42llc/dotnet-webca
```
#### On Windows:
```
$ docker run -it -p 8080:80 -v %UserProfile%/Desktop/localCA/:/var/localCA/ 
	--memory=40m division42llc/dotnet-webca
```

## Using the application
As of this writing, this is not fully-working, yet. You can create/delete/re-create the CA, 
and it will list certificates in the leaf folder. However, you can't create a leaf certificate
without getting an error.

### Screenshots
Below are some screenshots of the application.

![Home - without a CA set up](docs/screenshots/home-without-CA.png?raw=true "Home - without a CA set up")
![CA - create new CA certificate](docs/screenshots/CA-setup.png?raw=true "CA - create new CA certificate")
![CA - view CA certificate](docs/screenshots/CA-view.png?raw=true "CA - view CA certificate")
![Home - with a CA set up](docs/screenshots/home-with-CA.png?raw=true "Home - with a CA set up")
![Leaf - create a new leaf certificate](docs/screenshots/leaf-create.png?raw=true "Leaf - create a new leaf certificate")
![Leaf - list of leaf certificates](docs/screenshots/leaf-list.png?raw=true "Leaf - list of leaf certificates")
