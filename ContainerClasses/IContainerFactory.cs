﻿using CommonBasicStandardLibraries.CollectionClasses;
using System;
using System.Collections.Generic;
using System.Text;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
namespace CommonBasicStandardLibraries.ContainerClasses
{

    //risk doing without factory
    //because i ended up not using the factory anyways.  just added unnecessary complexity to the processes.


    //public interface IContainerFactory
    //{
    //    object GetReturnObject(CustomBasicList<ContainerData> PossibleResults, ContainerData ThisCurrent); //hopefully this works.
    //    //if you don't need it then ignore.
    //    //otherwise, too difficult to maintain in a different way.
    //    //object GetReturnObject(Dictionary<SendInfo, ResultInfo> ThisDict);
    //    //was going to be a dictionary but that does not work so well.


    //    object GetReturnObject(CustomBasicList<ContainerData> PossibleResults, Type TypeRequested); //if there is no match here, this will raise the exception.

    //    bool CanAcceptObject(CustomBasicList<ContainerData> PossibleResults, Type TypeRequested); //that way if none accept, then will raise exception.
    //}



    //public interface IAdvancedDIContainer //decided to do it this way so something else can populate it if needed.
    //{
    //    IAdvancedResolve MainContainer { get; set; }
    //}
    //public interface IRegisterContainer
    //{
    //    //this is everything to do with registering.
    //    //this means this can be used for an extension.
    //    void RegisterSingleton<TIn, TOut>() where TOut : TIn;


    //    void RegisterSingleton(Type ThisType); //this means that you will register one type as singleton.
    //    //void RegisterType<TIn>(bool IsSingleton); //i think it should still have the issingleton.
    //}
}