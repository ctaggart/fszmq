(*** do-not-eval-file ***)
(*** hide ***)
#I "../../../bin"

type ENV = System.Environment

let zmqVersion = if ENV.Is64BitProcess then "x64" else "x86"
ENV.CurrentDirectory <- sprintf "%s../../../../bin/zeromq/%s" __SOURCE_DIRECTORY__ zmqVersion

(**
Pub-Sub Message Evelopes
====================

Subscriber only wants messages whose topic frame is "B"
*)
#r "fszmq.dll"
open fszmq
open System.Text

let s_recv = Socket.recv >> Encoding.ASCII.GetString

let main () = 
  // prepare our context and subscriber
  use context     = new Context ()
  use subscriber  = Context.sub context
  Socket.connect subscriber "tcp://localhost:5563"
  Socket.subscribe subscriber [| "B"B |]

  while true do
    // read envelope with address
    let address = s_recv subscriber
    // read message contents
    let contents = s_recv subscriber
    printfn "[%s] %s" address contents
 
  0 // return code

(*** hide ***)
main ()
