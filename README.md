# golang-panic-parser
Panic stacktrace analyzer (written in C# :) ).

Helps find goroutines that get massively deadlocked, useful in tackling mem leaks.

## Usage

### Prepare dump

```
./<your-golang-app> > dump.txt 2>&1
---
kill -SIGABRT <pid>
```

### Process dump
This will output the common stacktraces with their number of occurences in the dump (high numbers may be stuck goroutines contributing to leaks):
```
dotnet run -- <path-to-dump.txt>
```

Sample output:
```json
[
  {
    "Key": "main.(*PacketParserImpl).listen(***, ***, ***, ***, ***, ***, ***, ***, ***, ***, ...)\n\t/go/src/sample-app/sample-app/packets_parser.go:39 +***\nmain.NewPacketParser.func1(***, ***, ***, ***, ***, ***, ***, ***, ***, ***, ...)\n\t/go/src/sample-app/sample-app/packets_parser.go:29 +***\ncreated by main.NewPacketParser\n\t/go/src/sample-app/sample-app/packets_parser.go:30 +***\n",
    "Value": 1027393
  },
  {
    "Key": "main.NewAutoflushingTcpAssembler.func1(***, ***, ***, ***)\n\t/go/src/sample-app/sample-app/packets.go:199 +***\ncreated by main.NewAutoflushingTcpAssembler\n\t/go/src/sample-app/sample-app/packets.go:211 +***\n",
    "Value": 3
  },
  {
    "Key": "sync.runtime_notifyListWait(***, ***)\n\t/usr/local/go/src/runtime/sema.go:267 +***\nsync.(*Cond).Wait(***)\n\t/usr/local/go/src/sync/cond.go:57 +***\nio.(*pipe).read(***, ***, ***, ***, ***, ***, ***)\n\t/usr/local/go/src/io/pipe.go:47 +***\nio.(*PipeReader).Read(***, ***, ***, ***, ***, ***, ***)\n\t/usr/local/go/src/io/pipe.go:129 +***\nbufio.(*Reader).fill(***)\n\t/usr/local/go/src/bufio/bufio.go:97 +***\nbufio.(*Reader).ReadSlice(***, ***, ***, ***, ***, ***, ***)\n\t/usr/local/go/src/bufio/bufio.go:330 +***\nbufio.(*Reader).ReadBytes(***, ***, ***, ***, ***, ***, ***)\n\t/usr/local/go/src/bufio/bufio.go:408 +***\nbufio.(*Reader).ReadString(***, ***, ***, ***, ***, ***)\n\t/usr/local/go/src/bufio/bufio.go:448 +***\nsipsentry/sample-app/vendor/github.com/dpoznyak/gossip/parser.(*parserBuffer).NextLine(***, ***, ***, ***, ***)\n\t/go/src/sample-app/sample-app/vendor/github.com/dpoznyak/gossip/parser/parserbuffer.go:46 +***\nsipsentry/sample-app/vendor/github.com/dpoznyak/gossip/parser.(*parser).parse(***, ***)\n\t/go/src/sample-app/sample-app/vendor/github.com/dpoznyak/gossip/parser/parser.go:175 +***\ncreated by sample-app/sample-app/vendor/github.com/dpoznyak/gossip/parser.NewParser\n\t/go/src/sample-app/sample-app/vendor/github.com/dpoznyak/gossip/parser/parser.go:122 +***\n",
    "Value": 1027357
  },
  {
    "Key": "sample-app/sample-app/vendor/github.com/dpoznyak/gossip/utils.(*ElasticChan).manage(***)\n\t/go/src/sample-app/sample-app/vendor/github.com/dpoznyak/gossip/utils/elasticchan.go:50 +***\ncreated by sample-app/sample-app/vendor/github.com/dpoznyak/gossip/utils.(*ElasticChan).Init\n\t/go/src/sample-app/sample-app/vendor/github.com/dpoznyak/gossip/utils/elasticchan.go:24 +***\n",
    "Value": 1027330
  },
  {
    "Key": "sync.runtime_notifyListWait(***, ***)\n\t/usr/local/go/src/runtime/sema.go:267 +***\nsync.(*Cond).Wait(***)\n\t/usr/local/go/src/sync/cond.go:57 +***\nio.(*pipe).read(***, ***, ***, ***, ***, ***, ***)\n\t/usr/local/go/src/io/pipe.go:47 +***\nio.(*PipeReader).Read(***, ***, ***, ***, ***, ***, ***)\n\t/usr/local/go/src/io/pipe.go:129 +***\nbufio.(*Reader).fill(***)\n\t/usr/local/go/src/bufio/bufio.go:97 +***\nbufio.(*Reader).ReadByte(***, ***, ***, ***)\n\t/usr/local/go/src/bufio/bufio.go:231 +***\nsipsentry/sample-app/vendor/github.com/dpoznyak/gossip/parser.(*parserBuffer).NextLine(***, ***, ***, ***, ***)\n\t/go/src/sample-app/sample-app/vendor/github.com/dpoznyak/gossip/parser/parserbuffer.go:53 +***\nsipsentry/sample-app/vendor/github.com/dpoznyak/gossip/parser.(*parser).parse(***, ***)\n\t/go/src/sample-app/sample-app/vendor/github.com/dpoznyak/gossip/parser/parser.go:175 +***\ncreated by sample-app/sample-app/vendor/github.com/dpoznyak/gossip/parser.NewParser\n\t/go/src/sample-app/sample-app/vendor/github.com/dpoznyak/gossip/parser/parser.go:122 +***\n",
    "Value": 36
  }
]
```

