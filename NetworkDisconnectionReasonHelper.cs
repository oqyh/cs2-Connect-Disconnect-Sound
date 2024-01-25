public enum ENetworkDisconnectionReason
{
    NETWORK_DISCONNECT_INVALID = 0, // Invalid 
    NETWORK_DISCONNECT_SHUTDOWN = 1, // Server Shutdown
    NETWORK_DISCONNECT_DISCONNECT_BY_USER = 2, // "#GameUI_Disconnect_User"
    NETWORK_DISCONNECT_DISCONNECT_BY_SERVER = 3, // "#GameUI_Disconnect_Server"
    NETWORK_DISCONNECT_LOST = 4, // "#GameUI_Disconnect_ConnectionLost"
    NETWORK_DISCONNECT_OVERFLOW = 5, // "#GameUI_Disconnect_ConnectionOverflow"
    NETWORK_DISCONNECT_STEAM_BANNED = 6, // "#GameUI_Disconnect_SteamIDBanned"
    NETWORK_DISCONNECT_STEAM_INUSE = 7, // "#GameUI_Disconnect_SteamIDInUse"
    NETWORK_DISCONNECT_STEAM_TICKET = 8, // "#GameUI_Disconnect_SteamTicket"
    NETWORK_DISCONNECT_STEAM_LOGON = 9, // "#GameUI_Disconnect_SteamLogon"
    NETWORK_DISCONNECT_STEAM_AUTHCANCELLED = 10, // "#GameUI_Disconnect_SteamLogon"
    NETWORK_DISCONNECT_STEAM_AUTHALREADYUSED = 11, // "#GameUI_Disconnect_SteamLogon"
    NETWORK_DISCONNECT_STEAM_AUTHINVALID = 12, // "#GameUI_Disconnect_SteamLogon"
    NETWORK_DISCONNECT_STEAM_VACBANSTATE = 13, // "#GameUI_Disconnect_SteamVAC"
    NETWORK_DISCONNECT_STEAM_LOGGED_IN_ELSEWHERE = 14, // "#GameUI_Disconnect_SteamInUse"
    NETWORK_DISCONNECT_STEAM_VAC_CHECK_TIMEDOUT = 15, // "#GameUI_Disconnect_SteamTimeOut"
    NETWORK_DISCONNECT_STEAM_DROPPED = 16, // "#GameUI_Disconnect_SteamDropped"
    NETWORK_DISCONNECT_STEAM_OWNERSHIP = 17, // "#GameUI_Disconnect_SteamOwnership"
    NETWORK_DISCONNECT_SERVERINFO_OVERFLOW = 18, // "#GameUI_Disconnect_ServerInfoOverflow"
    NETWORK_DISCONNECT_TICKMSG_OVERFLOW = 19, // "#GameUI_Disconnect_TickMessage"
    NETWORK_DISCONNECT_STRINGTABLEMSG_OVERFLOW = 20, // "#GameUI_Disconnect_StringTableMessage"
    NETWORK_DISCONNECT_DELTAENTMSG_OVERFLOW = 21, // "#GameUI_Disconnect_DeltaEntMessage"
    NETWORK_DISCONNECT_TEMPENTMSG_OVERFLOW = 22, // "#GameUI_Disconnect_TempEntMessage"
    NETWORK_DISCONNECT_SOUNDSMSG_OVERFLOW = 23, // "#GameUI_Disconnect_SoundsMessage"
    NETWORK_DISCONNECT_SNAPSHOTOVERFLOW = 24, // "#GameUI_Disconnect_SnapshotOverflow"
    NETWORK_DISCONNECT_SNAPSHOTERROR = 25, // "#GameUI_Disconnect_SnapshotError"
    NETWORK_DISCONNECT_RELIABLEOVERFLOW = 26, // "#GameUI_Disconnect_ReliableOverflow"
    NETWORK_DISCONNECT_BADDELTATICK = 27, // "#GameUI_Disconnect_BadClientDeltaTick"
    NETWORK_DISCONNECT_NOMORESPLITS = 28, // "#GameUI_Disconnect_NoMoreSplits"
    NETWORK_DISCONNECT_TIMEDOUT = 29, // "#GameUI_Disconnect_TimedOut"
    NETWORK_DISCONNECT_DISCONNECTED = 30, // "#GameUI_Disconnect_Disconnected"
    NETWORK_DISCONNECT_LEAVINGSPLIT = 31, // "#GameUI_Disconnect_LeavingSplit"
    NETWORK_DISCONNECT_DIFFERENTCLASSTABLES = 32, // "#GameUI_Disconnect_DifferentClassTables"
    NETWORK_DISCONNECT_BADRELAYPASSWORD = 33, // "#GameUI_Disconnect_BadRelayPassword"
    NETWORK_DISCONNECT_BADSPECTATORPASSWORD = 34, // "#GameUI_Disconnect_BadSpectatorPassword"
    NETWORK_DISCONNECT_HLTVRESTRICTED = 35, // "#GameUI_Disconnect_HLTVRestricted"
    NETWORK_DISCONNECT_NOSPECTATORS = 36, // "#GameUI_Disconnect_NoSpectators"
    NETWORK_DISCONNECT_HLTVUNAVAILABLE = 37, // "#GameUI_Disconnect_HLTVUnavailable"
    NETWORK_DISCONNECT_HLTVSTOP = 38, // "#GameUI_Disconnect_HLTVStop"
    NETWORK_DISCONNECT_KICKED = 39, // "#GameUI_Disconnect_Kicked"
    NETWORK_DISCONNECT_BANADDED = 40, // "#GameUI_Disconnect_BanAdded"
    NETWORK_DISCONNECT_KICKBANADDED = 41, // "#GameUI_Disconnect_KickBanAdded"
    NETWORK_DISCONNECT_HLTVDIRECT = 42, // "#GameUI_Disconnect_HLTVDirect"
    NETWORK_DISCONNECT_PURESERVER_CLIENTEXTRA = 43, // "#GameUI_Disconnect_PureServer_ClientExtra"
    NETWORK_DISCONNECT_PURESERVER_MISMATCH = 44, // "#GameUI_Disconnect_PureServer_Mismatch"
    NETWORK_DISCONNECT_USERCMD = 45, // "#GameUI_Disconnect_UserCmd"
    NETWORK_DISCONNECT_REJECTED_BY_GAME = 46, // "#GameUI_Disconnect_RejectedByGame"
    NETWORK_DISCONNECT_MESSAGE_PARSE_ERROR = 47, // "#GameUI_Disconnect_MessageParseError"
    NETWORK_DISCONNECT_INVALID_MESSAGE_ERROR = 48, // "#GameUI_Disconnect_InvalidMessageError"
    NETWORK_DISCONNECT_BAD_SERVER_PASSWORD = 49, // "#GameUI_Disconnect_BadServerPassword"
    NETWORK_DISCONNECT_DIRECT_CONNECT_RESERVATION = 50,
    NETWORK_DISCONNECT_CONNECTION_FAILURE = 51, // "#GameUI_Disconnect_ConnectionFailure"
    NETWORK_DISCONNECT_NO_PEER_GROUP_HANDLERS = 52, // "#GameUI_Disconnect_NoPeerGroupHandlers"
    NETWORK_DISCONNECT_RECONNECTION = 53,
    NETWORK_DISCONNECT_LOOPSHUTDOWN = 54, // "#GameUI_Disconnect_LoopShutdown"
    NETWORK_DISCONNECT_LOOPDEACTIVATE = 55, // "#GameUI_Disconnect_LoopDeactivate"
    NETWORK_DISCONNECT_HOST_ENDGAME = 56, // "#GameUI_Disconnect_Host_EndGame"
    NETWORK_DISCONNECT_LOOP_LEVELLOAD_ACTIVATE = 57, // "#GameUI_Disconnect_LoopLevelLoadActivate"
    NETWORK_DISCONNECT_CREATE_SERVER_FAILED = 58, // "#GameUI_Disconnect_CreateServerFailed"
    NETWORK_DISCONNECT_EXITING = 59, // "#GameUI_Disconnect_ExitingEngine"
    NETWORK_DISCONNECT_REQUEST_HOSTSTATE_IDLE = 60, // "#GameUI_Disconnect_Request_HSIdle"
    NETWORK_DISCONNECT_REQUEST_HOSTSTATE_HLTVRELAY = 61, // "#GameUI_Disconnect_Request_HLTVRelay"
    NETWORK_DISCONNECT_CLIENT_CONSISTENCY_FAIL = 62, // "#GameUI_ClientConsistencyFail"
    NETWORK_DISCONNECT_CLIENT_UNABLE_TO_CRC_MAP = 63, // "#GameUI_ClientUnableToCRCMap"
    NETWORK_DISCONNECT_CLIENT_NO_MAP = 64, // "#GameUI_ClientNoMap"
    NETWORK_DISCONNECT_CLIENT_DIFFERENT_MAP = 65, // "#GameUI_ClientDifferentMap"
    NETWORK_DISCONNECT_SERVER_REQUIRES_STEAM = 66, // "#GameUI_ServerRequireSteams"
    NETWORK_DISCONNECT_STEAM_DENY_MISC = 67, // "#GameUI_Disconnect_SteamDeny_Misc"
    NETWORK_DISCONNECT_STEAM_DENY_BAD_ANTI_CHEAT = 68, // "#GameUI_Disconnect_SteamDeny_BadAntiCheat"
    NETWORK_DISCONNECT_SERVER_SHUTDOWN = 69, // "#GameUI_Disconnect_ServerShutdown"
    NETWORK_DISCONNECT_REPLAY_INCOMPATIBLE = 71, // "#GameUI_Disconnect_ReplayIncompatible"
    NETWORK_DISCONNECT_CONNECT_REQUEST_TIMEDOUT = 72, // "#GameUI_Disconnect_ConnectionTimedout"
    NETWORK_DISCONNECT_SERVER_INCOMPATIBLE = 73, // "#GameUI_Disconnect_ServerIncompatible"
    NETWORK_DISCONNECT_LOCALPROBLEM_MANYRELAYS = 74, // "#GameUI_Disconnect_LocalProblem_ManyRelays"
    NETWORK_DISCONNECT_LOCALPROBLEM_HOSTEDSERVERPRIMARYRELAY = 75, // "#GameUI_Disconnect_LocalProblem_HostedServerPrimaryRelay"
    NETWORK_DISCONNECT_LOCALPROBLEM_NETWORKCONFIG = 76, // "#GameUI_Disconnect_LocalProblem_NetworkConfig"
    NETWORK_DISCONNECT_LOCALPROBLEM_OTHER = 77, // "#GameUI_Disconnect_LocalProblem_Other"
    NETWORK_DISCONNECT_REMOTE_TIMEOUT = 79, // "#GameUI_Disconnect_RemoteProblem_Timeout"
    NETWORK_DISCONNECT_REMOTE_TIMEOUT_CONNECTING = 80, // "#GameUI_Disconnect_RemoteProblem_TimeoutConnecting"
    NETWORK_DISCONNECT_REMOTE_OTHER = 81, // "#GameUI_Disconnect_RemoteProblem_Other"
    NETWORK_DISCONNECT_REMOTE_BADCRYPT = 82, // "#GameUI_Disconnect_RemoteProblem_BadCrypt"
    NETWORK_DISCONNECT_REMOTE_CERTNOTTRUSTED = 83, // "#GameUI_Disconnect_RemoteProblem_BadCert"
    NETWORK_DISCONNECT_UNUSUAL = 84, // "#GameUI_Disconnect_Unusual"
    NETWORK_DISCONNECT_INTERNAL_ERROR = 85, // "#GameUI_Disconnect_InternalError"
    NETWORK_DISCONNECT_REJECT_BADCHALLENGE = 128, // "#GameUI_ServerRejectBadChallenge"
    NETWORK_DISCONNECT_REJECT_NOLOBBY = 129, // "#GameUI_ServerNoLobby"
    NETWORK_DISCONNECT_REJECT_BACKGROUND_MAP = 130, // "#Valve_Reject_Background_Map"
    NETWORK_DISCONNECT_REJECT_SINGLE_PLAYER = 131, // "#Valve_Reject_Single_Player"
    NETWORK_DISCONNECT_REJECT_HIDDEN_GAME = 132, // "#Valve_Reject_Hidden_Game"
    NETWORK_DISCONNECT_REJECT_LANRESTRICT = 133, // "#GameUI_ServerRejectLANRestrict"
    NETWORK_DISCONNECT_REJECT_BADPASSWORD = 134, // "#GameUI_ServerRejectBadPassword"
    NETWORK_DISCONNECT_REJECT_SERVERFULL = 135, // "#GameUI_ServerRejectServerFull"
    NETWORK_DISCONNECT_REJECT_INVALIDRESERVATION = 136, // "#GameUI_ServerRejectInvalidReservation"
    NETWORK_DISCONNECT_REJECT_FAILEDCHANNEL = 137, // "#GameUI_ServerRejectFailedChannel"
    NETWORK_DISCONNECT_REJECT_CONNECT_FROM_LOBBY = 138, // "#Valve_Reject_Connect_From_Lobby"
    NETWORK_DISCONNECT_REJECT_RESERVED_FOR_LOBBY = 139, // "#Valve_Reject_Reserved_For_Lobby"
    NETWORK_DISCONNECT_REJECT_INVALIDKEYLENGTH = 140, // "#GameUI_ServerReject_InvalidKeyLength"
    NETWORK_DISCONNECT_REJECT_OLDPROTOCOL = 141, // "#GameUI_ServerRejectOldProtocol"
    NETWORK_DISCONNECT_REJECT_NEWPROTOCOL = 142, // "#GameUI_ServerRejectNewProtocol"
    NETWORK_DISCONNECT_REJECT_INVALIDCONNECTION = 143, // "#GameUI_ServerRejectInvalidConnection"
    NETWORK_DISCONNECT_REJECT_INVALIDCERTLEN = 144, // "#GameUI_ServerRejectInvalidCertLen"
    NETWORK_DISCONNECT_REJECT_INVALIDSTEAMCERTLEN = 145, // "#GameUI_ServerRejectInvalidSteamCertLen"
    NETWORK_DISCONNECT_REJECT_STEAM = 146, // "#GameUI_ServerRejectSteam"
    NETWORK_DISCONNECT_REJECT_SERVERAUTHDISABLED = 147, // "#GameUI_ServerAuthDisabled"
    NETWORK_DISCONNECT_REJECT_SERVERCDKEYAUTHINVALID = 148, // "#GameUI_ServerCDKeyAuthInvalid"
    NETWORK_DISCONNECT_REJECT_BANNED = 149, // "#GameUI_ServerRejectBanned"
    NETWORK_DISCONNECT_KICKED_TEAMKILLING = 150, // "#Player_DisconnectReason_TeamKilling"
    NETWORK_DISCONNECT_KICKED_TK_START = 151, // "#Player_DisconnectReason_TK_Start"
    NETWORK_DISCONNECT_KICKED_UNTRUSTEDACCOUNT = 152, // "#Player_DisconnectReason_UntrustedAccount"
    NETWORK_DISCONNECT_KICKED_CONVICTEDACCOUNT = 153, // "#Player_DisconnectReason_ConvictedAccount"
    NETWORK_DISCONNECT_KICKED_COMPETITIVECOOLDOWN = 154, // "#Player_DisconnectReason_CompetitiveCooldown"
    NETWORK_DISCONNECT_KICKED_TEAMHURTING = 155, // "#Player_DisconnectReason_TeamHurting"
    NETWORK_DISCONNECT_KICKED_HOSTAGEKILLING = 156, // "#Player_DisconnectReason_HostageKilling"
    NETWORK_DISCONNECT_KICKED_VOTEDOFF = 157, // "#Player_DisconnectReason_VotedOff"
    NETWORK_DISCONNECT_KICKED_IDLE = 158, // "#Player_DisconnectReason_Idle"
    NETWORK_DISCONNECT_KICKED_SUICIDE = 159, // "#Player_DisconnectReason_Suicide"
    NETWORK_DISCONNECT_KICKED_NOSTEAMLOGIN = 160, // "#Player_DisconnectReason_NoSteamLogin"
    NETWORK_DISCONNECT_KICKED_NOSTEAMTICKET = 161, // "#Player_DisconnectReason_NoSteamTicket"
}
public static class NetworkDisconnectionReasonHelper
{
    public static string GetDisconnectReasonString(ENetworkDisconnectionReason reason)
    {
        switch (reason)
        {
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_INVALID:
                return "Invalid Reason.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_SHUTDOWN:
                return "Server Shutdown.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_DISCONNECT_BY_USER:
                return "Disconnected by user.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_DISCONNECT_BY_SERVER:
                return "Disconnected from Server.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_LOST:
                return "Connection lost.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_OVERFLOW:
                return "Overflow error.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_STEAM_BANNED:
                return "STEAM UserID is banned.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_STEAM_INUSE:
                return "STEAM UserID is already in use on this server.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_STEAM_TICKET:
                return "STEAM UserID ticket is invalid.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_STEAM_LOGON:
                return "No Steam logon.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_STEAM_AUTHCANCELLED:
                return "Steam authorization failed. Connection to server denied by Steam.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_STEAM_AUTHALREADYUSED:
                return "Steam authorization failed. Connection to server denied by Steam.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_STEAM_AUTHINVALID:
                return "Steam authorization failed. Connection to server denied by Steam.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_STEAM_VACBANSTATE:
                return "VAC banned from secure server.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_STEAM_LOGGED_IN_ELSEWHERE:
                return "This Steam account is being used in another location.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_STEAM_VAC_CHECK_TIMEDOUT:
                return "Valve Anti-Cheat challenge timed out. Please ensure that you are not using any programs that may interfere with VAC, and confirm that Steam is correctly installed.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_STEAM_DROPPED:
                return "Steam authorization failed. You must be connected to Steam to make the initial connection to the game server.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_STEAM_OWNERSHIP:
                return "This Steam account does not own this game. Please login to the correct Steam account.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_SERVERINFO_OVERFLOW:
                return "Info data overflow.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_TICKMSG_OVERFLOW:
                return "Server failed to write client tick message.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_STRINGTABLEMSG_OVERFLOW:
                return "Error writing string table update message.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_DELTAENTMSG_OVERFLOW:
                return "Too many entities on the server.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_TEMPENTMSG_OVERFLOW:
                return "Too many temporary entities on the server.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_SOUNDSMSG_OVERFLOW:
                return "Error reliable buffer overflow.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_SNAPSHOTOVERFLOW:
                return "Error reliable snapshot overflow.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_SNAPSHOTERROR:
                return "Error sending snapshot.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_RELIABLEOVERFLOW:
                return "Error reliable buffer overflow.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_BADDELTATICK:
                return "Client delta ticks out of order.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_NOMORESPLITS:
                return "No more split slots available.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_TIMEDOUT:
                return "Unable to establish a connection with the gameserver.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_DISCONNECTED:
                return "Disconnected. The console might have more details.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_LEAVINGSPLIT:
                return "Split slot disconnected.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_DIFFERENTCLASSTABLES:
                return "Server uses different class tables.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_BADRELAYPASSWORD:
                return "Bad relay password.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_BADSPECTATORPASSWORD:
                return "Bad spectator password.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_HLTVRESTRICTED:
                return "SourceTV server is restricted to local spectators (class C).";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_NOSPECTATORS:
                return "Match does not allow spectators.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_HLTVUNAVAILABLE:
                return "No SourceTV relay available.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_HLTVSTOP:
                return "SourceTV stop.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_KICKED:
                return "Kicked by server.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_BANADDED:
                return "Added to banned list.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_KICKBANADDED:
                return "Kicked and banned.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_HLTVDIRECT:
                return "SourceTV cannot connect to the game directly.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_PURESERVER_CLIENTEXTRA:
                return "Pure server: client has loaded extra file(s).";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_PURESERVER_MISMATCH:
                return "Pure server: client file does not match server.\n\nhttps://support.steampowered.com/kb_article.php?ref=8285-YOAZ-6049";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_USERCMD:
                return "Error in parsing user commands.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REJECTED_BY_GAME:
                return "Connection rejected by game.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_MESSAGE_PARSE_ERROR:
                return "Failed to parse message.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_INVALID_MESSAGE_ERROR:
                return "Message is invalid.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_BAD_SERVER_PASSWORD:
                return "Bad password rejected by server.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_DIRECT_CONNECT_RESERVATION:
                return "Reservation cannot connect to the game directly.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_CONNECTION_FAILURE:
                return "Connection failure.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_NO_PEER_GROUP_HANDLERS:
                return "Connection peer group missing.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_RECONNECTION:
                return "Reconnecting";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_LOOPSHUTDOWN:
                return "Loop shutdown.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_LOOPDEACTIVATE:
                return "Loop deactivated.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_HOST_ENDGAME:
                return "Game ended by host.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_LOOP_LEVELLOAD_ACTIVATE:
                return "Loop level loading activated.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_CREATE_SERVER_FAILED:
                return "Failed to create server.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_EXITING:
                return "Shutting down game.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REQUEST_HOSTSTATE_IDLE:
                return "Host is idle.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REQUEST_HOSTSTATE_HLTVRELAY:
                return "Host is a relay.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_CLIENT_CONSISTENCY_FAIL:
                return "Client consistency check failed.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_CLIENT_UNABLE_TO_CRC_MAP:
                return "Client map verification failed.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_CLIENT_NO_MAP:
                return "Required map is missing on your client.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_CLIENT_DIFFERENT_MAP:
                return "Your map version differs from the server.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_SERVER_REQUIRES_STEAM:
                return "Client and server must be connected to Steam.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_STEAM_DENY_MISC:
                return "Connection to server denied by Steam.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_STEAM_DENY_BAD_ANTI_CHEAT:
                return "Connection to server denied by VAC.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_SERVER_SHUTDOWN:
                return "Disconnected from gameserver. The gameserver is shutting down.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REPLAY_INCOMPATIBLE:
                return "Replay is not compatible.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_CONNECT_REQUEST_TIMEDOUT:
                return "Unable to establish a connection with the gameserver.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_SERVER_INCOMPATIBLE:
                return "Server version is not compatible.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_LOCALPROBLEM_MANYRELAYS:
                return "Lost connection, even after trying several relays in different geographic locations. The most likely cause is a problem with your Internet connection.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_LOCALPROBLEM_HOSTEDSERVERPRIMARYRELAY:
                return "Gameserver has lost connectivity with the primary relay the client was using.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_LOCALPROBLEM_NETWORKCONFIG:
                return "Check your Internet connection. Unable to download the network configuration from CDN.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_LOCALPROBLEM_OTHER:
                return "Disconnected. It looks like there may be a problem with your Internet connection.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REMOTE_TIMEOUT:
                return "The game stopped receiving communications from the remote host.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REMOTE_TIMEOUT_CONNECTING:
                return "After several attempts to connect, the server did not respond.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REMOTE_OTHER:
                return "Problem communicating with the remote host.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REMOTE_BADCRYPT:
                return "The remote host presented a bad certificate or is misconfigured.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REMOTE_CERTNOTTRUSTED:
                return "The remote host presented a certificate that could not be used for authentication.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_UNUSUAL:
                return "Disconnected. The console might have more details.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_INTERNAL_ERROR:
                return "Disconnected due to an internal error. The console might have more details.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REJECT_BADCHALLENGE:
                return "Bad challenge packet rejected by server.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REJECT_NOLOBBY:
                return "Server is not hosting a game lobby.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REJECT_BACKGROUND_MAP:
                return "Server is running a background map.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REJECT_SINGLE_PLAYER:
                return "Server is running a single player game.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REJECT_HIDDEN_GAME:
                return "Server is running a hidden game.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REJECT_LANRESTRICT:
                return "Server is restricted to LAN games only.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REJECT_BADPASSWORD:
                return "Bad password rejected by server.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REJECT_SERVERFULL:
                return "Server is full.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REJECT_INVALIDRESERVATION:
                return "Server reservation is invalid.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REJECT_FAILEDCHANNEL:
                return "Bad network channel rejected by server.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REJECT_CONNECT_FROM_LOBBY:
                return "Server requires clients to connect from a game lobby.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REJECT_RESERVED_FOR_LOBBY:
                return "Server is reserved for a game lobby.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REJECT_INVALIDKEYLENGTH:
                return "Invalid key rejected by server.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REJECT_OLDPROTOCOL:
                return "Your client is out of date.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REJECT_NEWPROTOCOL:
                return "Server is out of date.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REJECT_INVALIDCONNECTION:
                return "Invalid connection rejected by server.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REJECT_INVALIDCERTLEN:
                return "Invalid client certificate rejected by server.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REJECT_INVALIDSTEAMCERTLEN:
                return "Invalid Steam certificate rejected by server.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REJECT_STEAM:
                return "Steam rejected your connection to server.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REJECT_SERVERAUTHDISABLED:
                return "Server authentication is disabled.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REJECT_SERVERCDKEYAUTHINVALID:
                return "CD key authentication rejected by server.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_REJECT_BANNED:
                return "Your client is not allowed to join this server.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_KICKED_TEAMKILLING:
                return "For killing too many teammates.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_KICKED_TK_START:
                return "For killing a teammate at round start.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_KICKED_UNTRUSTEDACCOUNT:
                return "Account is Untrusted.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_KICKED_CONVICTEDACCOUNT:
                return "Account is Convicted.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_KICKED_COMPETITIVECOOLDOWN:
                return "Player has competitive matchmaking cooldown.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_KICKED_TEAMHURTING:
                return "For doing too much team damage.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_KICKED_HOSTAGEKILLING:
                return "For killing too many hostages.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_KICKED_VOTEDOFF:
                return "Voted off.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_KICKED_IDLE:
                return "Player idle.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_KICKED_SUICIDE:
                return "For suiciding too many times.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_KICKED_NOSTEAMLOGIN:
                return "No user logon.";
            case ENetworkDisconnectionReason.NETWORK_DISCONNECT_KICKED_NOSTEAMTICKET:
                return "Game authentication failed.";
            default:
                return $"Unknown Disconnect Reason {(int)reason}";
        }
    }
}