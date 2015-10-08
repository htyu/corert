//
// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//
// -----------------------------------------------------------------------------------------------------------
// Support for emitting debug events with particular payloads that a managed-aware debugger can listen for.
// The events are generated using 1st chance SEH exceptions that the debugger should immediately continue
// so the exception never dispatches back into runtime code. However just in case the debugger disconnects
// or doesn't behave well we've got a backstop catch handler that will prevent it from escaping the code in
// DebugEventSource.
// -----------------------------------------------------------------------------------------------------------

#ifndef __DEBUG_EVENT_SOURCE_H_
#define __DEBUG_EVENT_SOURCE_H_

// This global is set from out of process using the debugger. It controls which events are emitted.
GVAL_DECL(UInt32, g_DebuggerEventsFilter);

#ifndef DACCESS_COMPILE

// this is also defined in cordebug.h, but I don't want to pull that header into redhawk
// runtime build.
typedef UInt64 CORDB_ADDRESS;

struct DebugEventPayload;

class DebugEventSource
{
public:
    static void SendModuleLoadEvent(Module* pModule);
    static void SendModuleUnloadEvent(Module* pModule);
    static void SendExceptionThrownEvent(CORDB_ADDRESS faultingIP, CORDB_ADDRESS faultingFrameSP);
    static void SendExceptionCatchHandlerFoundEvent(CORDB_ADDRESS handlerIP, CORDB_ADDRESS HandlerFrameSP);
    static void SendExceptionUnhandledEvent();
    static void SendExceptionFirstPassFrameEnteredEvent(CORDB_ADDRESS ipInFrame, CORDB_ADDRESS frameSP);

private:
    static void SendRawEvent(DebugEventPayload* payload);
};


#endif //!DACCESS_COMPILE


#endif // __DEBUG_EVENT_SOURCE_H_