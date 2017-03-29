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
Below are some screenshots of the application. First, working with the CA, itself:

![Home - without a CA](docs/screenshots/home-without-CA.png?raw=true "Home - without a CA")
![CA - setup](docs/screenshots/CA-setup.png?raw=true "CA - setup")
![CA - View 1](docs/screenshots/CA-view1.png?raw=true "CA - View 1")
![CA - View 2](docs/screenshots/CA-view2.png?raw=true "CA - View 2")
![Home - with a CA](docs/screenshots/home-with-CA.png?raw=true "Home - with a CA")
![CA - delete](docs/screenshots/CA-delete.png?raw=true "CA - delete")

Then, working with leaf certificates, signed by the CA:

![Leaf - create leaf certificates](docs/screenshots/leaf-create.png?raw=true "Leaf - create leaf certificates")
![Leaf - list of leaf certificates](docs/screenshots/leaf-list.png?raw=true "Leaf - list of leaf certificates")
![Leaf - view 1 leaf certificates](docs/screenshots/leaf-view1.png?raw=true "Leaf - view 1 leaf certificates")
![Leaf - view 2 leaf certificates](docs/screenshots/leaf-view2.png?raw=true "Leaf - view 2 leaf certificates")
![Leaf - delete leaf certificates](docs/screenshots/leaf-delete.png?raw=true "Leaf - delete leaf certificates")

