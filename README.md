# NDigester
NDigester verifies that a file is correct by retrieving the hash value of the file or by specifying a hash value.

## Features

```
$ ndigester --help
Description:
  Obtains a hash value of a specified file or compares files by hash value.

Usage:
  ndigester <target> [options]

Arguments:
  <target>  Specifies the target file path from which hash values are to be obtained.

Options:
  -a, --algorithm <MD5|SHA1|SHA256|SHA384|SHA512>  Specifies the algorithm of the hash value. [default: MD5]
  -c, --compare <compare>                          Specifies a hash value to compare with the target file.
  --version                                        Show version information
  -?, -h, --help                                   Show help and usage information
```

### Get the hash value of a file
Outputs the hash value of a target file by specifying the file path as a command line argument.

```
$ ndigester path/to/file
0e4e9aa41d24221b29b19ba96c1a64d0
```

Use the specified hash algorithm by specifying `--algorithm <ALGORITHM_NAME>`. Specify the following hash algorithm. (default: `MD5` )

- MD5
- SHA1
- SHA256
- SHA384
- SHA512

```
$ ndigester path/to/file --algorithm sha256
5bfb6f3ab89e198539408f7e0e8ec0b0bd5efe8898573ec05b381228efb45a5d
```

### File Verification
The `--compare <HASH>` will compare the specified hash value with the hash value of the file path to verify that the file is correct.

```
$ ndigester path/to/file --algorithm sha256 --compare 5bfb6f3ab89e198539408f7e0e8ec0b0bd5efe8898573ec05b381228efb45a5d
True
```

# Author
[@zwei_222](https://twitter.com/zwei_222)

# License
This software is released under the MIT License, see LICENSE.
