.PHONY: compile compile-test update update-build update-test clean clean-build clean-test clean-packages

compile:
	xbuild TempoDB/TempoDB.csproj

compile-test: compile
	xbuild TempoDB.Tests/TempoDB.Tests.csproj

update-build:
	xbuild TempoDB/TempoDB.csproj /t:RestorePackages

update-test:
	xbuild TempoDB.Tests/TempoDB.Tests.csproj /t:RestorePackages

update: update-build update-test

test: compile-test
	mono packages/NUnit.Runners.2.6.1/tools/nunit-console.exe TempoDB.Tests/bin/Debug/TempoDB.Tests.dll

check: test

clean-build:
	rm -rf TempoDB/bin
	rm -rf TempoDB/obj

clean-test:
	rm -rf TempoDB.Tests/bin
	rm -rf TempoDB.Tests/obj

clean-packages:
	rm -rf packages

clean: clean-build clean-test
