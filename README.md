# Stubbl API

[![Build status](https://ci.appveyor.com/api/projects/status/dbn6brnkbi04g6hd/branch/master?svg=true)](https://ci.appveyor.com/project/stubbl/stubbl-api/branch/master)

## Endpoints

| Name | Method | URL |
| ---- | ------ | --- |
| **Health** |
| Canary | `GET` | `/canary` |
| Ping | `GET` | `/ping` |
| **Authenticated User** |
| Find Authenticated User | `GET`  | `/user/find` |
| Update Authenticated User | `POST` | `/user/update` |
| **Authenticated User Invitations** |
| Count Authenticated User Invitations | `GET` | `/user/invitations/count` |
| List Authenticated User Invitations | `GET` | `/user/invitations/list` |
| Accept Authenticated User Invitation | `POST` | `/user/invitations/:invitationId/accept` |
| Decline Authenticated User Invitation | `POST` | `/user/invitations/:invitationId/decline` |
| **Teams** |
| Count Teams | `GET` | `/teams/count` |
| List Teams | `GET` | `/teams/list` |
| Create Team | `POST` | `/teams/create` |
| Find Team | `GET` | `/teams/:teamId/find` |
| Update Team | `POST` | `/teams/:teamId/update` |
| Delete Team | `POST` | `/teams/:teamId/delete` |
| **Team Invitations** |
| Count Team Invitations | `GET` | `/teams/:teamId/invitations/count` |
| List Team Invitations | `GET` | `/teams/:teamId/invitations/list` |
| Create Team Invitation | `POST` | `/teams/:teamId/invitations/create` |
| Find Team Invitation | `GET` | `/teams/:teamId/invitations/:invitationId/find` |
| Delete Team Invitation | `POST` | `/teams/:teamId/invitations/:invitationId/delete` |
| Accept Team Invitation | `POST` | `/teams/:teamId/invitations/:invitationId/accept` |
| Decline Team Invitation | `POST` | `/teams/:teamId/invitations/:invitationId/decline` |
| **Team Logs** |
| Count Team Logs | `GET` | `/teams/:teamId/logs/count` |
| List Team Logs | `GET` | `/teams/:teamId/logs/list` |
| Find Team Log | `GET` | `/teams/:teamId/logs/:logId/find` |
| **Team Members** |
| Count Team Members | `GET` | `/teams/:teamId/members/count` |
| List Team Members | `GET` | `/teams/:teamId/members/list` |
| Find Team Member | `GET` | `/teams/:teamId/members/:memberId/find` |
| Remove Team Member | `POST` | `/teams/:teamId/members/:memberId/remove` |
| **Team Roles** |
| Count Team Roles | `GET` | `/teams/:teamId/roles/count` |
| List Team Roles | `GET` | `/teams/:teamId/roles/list` |
| Create Team Role | `POST` | `/teams/:teamId/roles/create` |
| Find Team Role | `GET` | `/teams/:teamId/roles/:roleId/find` |
| Update Team Role | `POST` | `/teams/:teamId/roles/:roleId/update` |
| Delete Team Role | `POST` | `/teams/:teamId/roles/:roleId/delete` |
| **Team Stubs** |
| Count Team Stubs | `GET` | `/teams/:teamId/stubs/count` |
| List Team Stubs | `GET` | `/teams/:teamId/stubs/list` |
| Create Team Stub | `POST` | `/teams/:teamId/stubs/create` |
| Find Team Stub | `GET` | `/teams/:teamId/stubs/:stubId/find` |
| Update Team Stub | `POST` | `/teams/:teamId/stubs/:stubId/update` |
| Delete Team Stub | `POST` | `/teams/:teamId/stubs/:stubId/delete` |
| Clone Team Stub | `POST` | `/teams/:teamId/stubs/:stubId/clone` |
| **Permissions** |
| Count Permissions | `GET` | `/permissions/count` |
| List Permissions | `GET` | `/permissions/list` |

## Development

You will need:

- MongoDB
- Storage Emulator
- Visual Studio ran as an Administrator (to allow for subdomains via [xip.io](http://xip.io))

The Stubbl API is secured by Identity Server, but you can bypass this during development by specifying a `X-Sub` header with a value corresponding the a user stored in MongoDB, which will be prepopulated with test data (e.g. `X-Sub: 1`). [Swagger](http://stubblapi.127.0.0.1.xip.io/swagger/) also supports the ability to specify the sub header.