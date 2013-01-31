.PHONY: compile compile-test update update-build update-test clean clean-build clean-test clean-packages

compile:
	xbuild TempoDB/TempoDB.csproj
	xbuild Client/TempoClient.csproj

compile-test: compile
	xbuild TempoDB.Tests/TempoDB.Tests.csproj
	xbuild Client.Tests/Client.Tests.csproj

update-build:
	xbuild TempoDB/TempoDB.csproj /t:RestorePackages
	xbuild Client/TempoClient.csproj /t:RestorePackages

update-test:
	xbuild TempoDB.Tests/TempoDB.Tests.csproj /t:RestorePackages
	xbuild Client.Tests/Client.Tests.csproj /t:RestorePackages

update: update-build update-test

test: compile-test
	mono packages/NUnit.Runners.2.6.1/tools/nunit-console.exe TempoDB.Tests/bin/Debug/TempoDB.Tests.dll
	mono packages/NUnit.Runners.2.6.1/tools/nunit-console.exe Client.Tests/bin/Debug/Client.Tests.dll

clean-build:
	rm -rf Client/bin
	rm -rf Client/obj
	rm -rf TempoDB/bin
	rm -rf TempoDB/obj

clean-test:
	rm -rf Client.Tests/bin
	rm -rf Client.Tests/obj
	rm -rf TempoDB.Tests/bin
	rm -rf TempoDB.Tests/obj

clean-packages:
	rm -rf packages

clean: clean-build clean-test
