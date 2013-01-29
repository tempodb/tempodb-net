.PHONY: compile compile-test update update-build update-test clean clean-build clean-test

compile:
	xbuild Client/TempoClient.csproj

compile-test: compile
	xbuild Client.Tests/Client.Tests.csproj

update-build:
	xbuild Client/TempoClient.csproj /t:RestorePackages

update-test:
	xbuild Client.Tests/Client.Tests.csproj /t:RestorePackages

update: update-build update-test

test: compile-test
	mono packages/NUnit.Runners.2.6.1/tools/nunit-console.exe Client.Tests/bin/Debug/Client.Tests.dll

clean-build:
	rm -rf Client/bin
	rm -rf Client/obj

clean-test:
	rm -rf Client.Tests/bin
	rm -rf Client.Tests/obj

clean: clean-build clean-test
