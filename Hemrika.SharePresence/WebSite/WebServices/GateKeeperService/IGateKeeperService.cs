// <copyright file="IGateKeeperService.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-04-24 11:44:16Z</date>
namespace Hemrika.SharePresence.WebSite.GateKeeper
{
    using System.ServiceModel;
    using Hemrika.SharePresence.WebSite.Modules.GateKeeper;
    using System;

    [ServiceContract]
    public interface IGateKeeperService
    {
        [OperationContract]
        bool HasListing(GateKeeperType type, GateKeeperListing node, string value);

        [OperationContract]
        void RemoveListing(string id);

        [OperationContract]
        void GateKeeper(GateKeeperType type, GateKeeperListing node, string value);

        [OperationContract]
        void HoneyPot(string UserHostAddress, string Referrer, string UserAgent);

        [OperationContract]
        void HTTP(string UserHostAddress, string LastActivity, string Referrer, string ThreatLevel, string UserAgent, string VisitorType);

        [OperationContract]
        void Drone(string UserHostAddress, string Referrer, string UserAgent);

        [OperationContract]
        void Proxy(string UserHostAddress, string Referrer, string UserAgent);
    }
}

