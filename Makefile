.PHONY: compile compile-test clean clean-build clean-test

compile:
	xbuild Client/TempoClient.csproj

compile-test: compile
	xbuild Client.Tests/Client.Tests.csproj

clean-build:
	rm -rf Client/bin
	rm -rf Client/obj

clean-test:
	rm -rf Client.Tests/bin
	rm -rf Client.Tests/obj

clean: clean-build clean-test
