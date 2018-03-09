# Comparer Service
Comparer Services is web app to perform content comparison.
## Notes
Service has [Swagger](https://swagger.io/) documentation enabled. To view it navigate to `/swagger`.
> Due to regression in latest swagger-ui version executing requests in IE Edge returns `Unknown response type`. Please use FireFox or Chrome.

## Instructions
### Run from binaries
* To run app from binaries you need [.NET Core Runtime 2.0.5](https://www.microsoft.com/net/download/Windows/run) installed.
* This configuraiton uses **in-memory** storage by default.

1. Get latest binaries from the [relases](https://github.com/me-viper/comparer/releases) page.
2. Unpack archive.
3. Run `dotnet .\ComparerService.App.dll --urls="http://localhost:55605"` for the project root.
4. Navigate to http://localhost:55605/swagger

### Run for the source code
* To run app from the souce code you need [.NET Core SDK 2.1.4](https://www.microsoft.com/net/download/windows) installed.
* This configuraiton uses **in-memory** storage by default.

1. Clone git repository `git clone https://github.com/me-viper/comparer.git`.
2. Run `run.ps1` from the project root.

Script will build project, start local server (by default at http://localhost:55603) and navigate your default browser to http://localhost:55603/swagger.

### Run unit and integration tests
* To run tests you need [.NET Core SDK 2.1.4](https://www.microsoft.com/net/download/windows) installed.
1. Clone git repository `git clone https://github.com/me-viper/comparer.git`.
2. Run `dotnet test .\tests\Comparer.Tests\` from the project root.

### Run in Docker
* To run app in docker container you need to have [Docker](https://store.docker.com/editions/community/docker-ce-desktop-windows) for Windows installed.
* App configuration for docker uses Linux. To run it you need to have your Docker configured to use Linux containers 
(see *Switch betweern Windows and Linux contaienrs* section in [Docker documentation](https://docs.docker.com/docker-for-windows/#switch-between-windows-and-linux-containers)).
* This configuraiton uses **Redis** storage by default.

1. Clone git repository `git clone https://github.com/me-viper/comparer.git`.
2. Run `docker.ps1` from the project root.

### Configuration options
`--urls="BASE URL"` - specifies base url for the service.
> Example: `--urls="http://localhost:8080"`

`--store="STORE NAME"` - allows to override default in-memory store. Possible options:
* `redis` - use Redis as content store.
> Example: `--store="redis"`

`--redis.endpoint="URL"` - overrides default (http://localhost:6379) Redis connection string. Ignored if `store` paramter is not `redis`.
> Example: `--store="redis" --redis.endpoint="http://localhost:9999"`

## API
The service exposes the following endpoints:
### POST /v1/diff/{id}/left
Sets left side of comparison.
* `id` *(path)* - comparison ID.
* `content` *(body)* - base64 encoded content to compare.
> Example: `curl -X POST "http://localhost/v1/Diff/123/left" -H "accept: application/json" -H "Content-Type: application/json" -d "\"dGhpcyBpcyBzYW1wbGUgdGV4dA==\""`
### POST /v1/diff/{id}/right
Sets right side of comparison.
* `id` *(path)* - comparison ID.
* `content` *(body)* - base64 encoded content to compare.
> Example: `curl -X POST "http://localhost/v1/Diff/123/left" -H "accept: application/json" -H "Content-Type: application/json" -d "\"dGhpcyBpcyBzYW1wbGUgdGV4dA==\""`
### GET /v1/diff/{id}
Compares left and right.
* `id` *(path)* - comparison ID.
> Example: `curl -X GET "http://localhost/v1/Diff/123" -H "accept: application/json"`