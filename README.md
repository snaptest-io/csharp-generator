# SnapTest C# Test Generator

Generates SnapTest Suites in C# via the [snaptest-cli](https://www.npmjs.com/package/snaptest-cli).

***If you are just getting started, use one of the following harnesses/scaffolds to get started:***
1. ***[C# project NUNIT harness](https://github.com/snaptest-io/csharpnunit-harness)***
1. ***[C# project XUNUT harness](https://github.com/snaptest-io/csharpxunit-harness)***

Contributions in the forms of PRs and tickets are welcome.

### How to build this generator

1. First, make sure you have the snaptest-cli tool installed by running `npm install -g snaptest-cli`.
1. Then use snaptest-cli's custom generator flag to utilize your own branch/fork of this generator:

`snaptest -c <absolute or relative path to generators index.js> ...`

#### Custom generator example:

`snaptest -c ./index.js -t iHrsRTzgENFUVI1TPAtIFqyd0QElssxy1TA0X9y`

1. The -c flag specifics a "custom" generator, which lets you use a local generator on your filesystem.  This replaces the -r flag which specifies an official generator.  
 
#### Reference

1. snaptest-cli reference at [snaptest-cli](https://www.npmjs.com/package/snaptest-cli)

 