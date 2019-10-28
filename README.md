EventLogBencher
===

[![Build status](https://ci.appveyor.com/api/projects/status/2j8806pj2ht3hxb9/branch/master?svg=true)](https://ci.appveyor.com/project/cosmo0920/eventlogbencher/branch/master)

A tiny EventLog benchmark tool.

### Prerequisites

#### Benchmark Tool

* .NET Framework 4.6.1 Runtime

#### Windows Monitoring Tool

* .Net Core 2.2

#### Linux Monitoring Tool

* Python3
  * psutil

### Environment

#### Benchmark Tool & Windows Monitoring Tool

* Confirmed to work with Windows 10 1809 or later

#### Linux Monitoring Tool

* Ubuntu 18.04 LTS

### Usage

#### Benchmark Tools

```powershell
PS> .\bin\<build type>\EventLogBencher.exe -w [wait milliseconds] -t [total emitting events] [-l [emitting lorem ipsum text length (1 to 65535)]]
```

```powershell
PS> .\bin\<build type>\FileLoggingBencher.exe -r [flow rate] -t [total emitting steps] [-l [emitting lorem ipsum text length (1 to 65535)]]
```

#### Monitor Tools

```bash
$ pip3 install -r BenchmarkPerformanceMonitorUnix/requirements.txt
$ python3 ./BenchmarkPerformanceMonitorUnix/monitor.py [steps]
```

### LICENSE

[MIT](LICENSE).
