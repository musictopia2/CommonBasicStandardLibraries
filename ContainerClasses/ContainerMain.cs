﻿using System;
using System.Text;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using System.Linq;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using CommonBasicStandardLibraries.CollectionClasses;
using System.Collections.Generic;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator;
//i think this is the most common things i like to do
namespace CommonBasicStandardLibraries.ContainerClasses
{
    public enum EnumResolveCategory
    {
        ShowError = 0,
        ManuelPriority = 1, //if priority is used, then you have to specify or still won't work. needs to be for all.
        FIFO = 2,
        LIFO = 3,
        //Factory = 4, //this means that whoever has a factory will get it first.  if more than one factory, then raise error so i have to rethink
    }
    //decided to noteven deal with factories.  because i don't think we even dealt with factories anyways.



    public class ContainerMain: IResolver //this is the main class for the container
    {
        private readonly HashSet<ContainerData> _thisSet = new HashSet<ContainerData>();
        public static EnumResolveCategory ResolveCategory = EnumResolveCategory.ShowError; //default to show error so you have to rethink.

        private int ID;
        //private static readonly HashSet<IContainerFactory> FactoryList = new HashSet<IContainerFactory>(); //i think i want anybody to be able to add to this list.  makes the container more powerful.
        //public IContainerFactory ParentFactory; //i think its implied that if you set the parent, the parent will handle all duplicates.
        private readonly RandomGenerator _rans; //this is most common.

        //decided to not even have the ability to clear container since game package had its own system.
        //its probably best its kept this way.  they both implement iresolver so when needed, works.


        //public void ClearContainer() //there are cases i still have to clear container (if doing multiple games and some has to re registered
        //{
        //    ThisSet.Clear();
        //    //FactoryList.Clear();
        //}

        public ContainerMain()
        {
            _rans = new RandomGenerator();
            //RegisterSingleton(Rans);
        }


        //public static void AddFactoryToContainer(IContainerFactory ThisFact)
        //{
        //    FactoryList.Add(ThisFact);
        //}

        public T GetInstance<T>()
        {
            if (typeof(T) == typeof(RandomGenerator)) //this is an exception.
            {
                //i originally had static random generators.  can't do anymore because of testing problems.  so this will do it instead.
                object thisObj = _rans;
                return (T)thisObj;
            }
            return (T)GetInstance(typeof(T));
        }
        public T GetInstance<T>(object tag)
        {
            return (T)SimpleInstance(typeof(T), tag);
        }
        private object SimpleInstance(Type thisType, object thisObject)
        {
            //this will only do simple ones period.
            CustomBasicList<ContainerData> tempList = _thisSet.Where(Items => Items.TypeIn == thisType && Items.ExtraDetails == thisObject).ToCustomBasicList();
            if (tempList.Count == 0)
                throw new BasicBlankException($"{thisType.Name} With Tag Not Found");
            //if you send in tag, it must match the tag that was registered with.
            if (tempList.Count > 1)
                throw new BasicBlankException($"Had Duplicates For Instance With Tag.  Name Was {thisType.Name}.  Rethink");
            return GetInstance(tempList.Single());
        }
        //private CustomBasicList<ContainerData> PossibleList(ContainerData thisResult, Type thisType)
        //{
        //    var TempList = _thisSet.Where(Items => Items.TypeIn == thisResult.TypeIn || Items.TypeOut == thisResult.TypeOut).ToCustomBasicList();
        //    TempList.ForEach(Items => Items.RequestedType = thisType);
        //    return TempList;
        //}

        private object GetInstance(ContainerData thisResult)
        {
            //if (ThisResult.ThisFact != null && ThisResult.SimpleFunction != null)
            //    throw new BasicBlankException("You can't both use a function and a factory");
            if (thisResult.ThisObject != null && thisResult.IsSingle == true)
                return thisResult.ThisObject;

            //if (ThisResult.ThisFact != null)
            //{
            //    if (ThisResult.IsSingle == true)
            //    {
            //        ThisResult.ThisObject = ThisResult.ThisFact.GetReturnObject(PossibleList(ThisResult, ThisType), ThisResult);
            //        return ThisResult.ThisObject;
            //    }
            //    else
            //        return ThisResult.ThisFact.GetReturnObject(PossibleList(ThisResult, ThisType), ThisResult); //since objects are going to be used, then its more flexible
            //    //the downside is it takes a while to get used to it.  not sure how i can use generics without causing other problems though.
            //}
            if (thisResult.SimpleFunction != null)
            {
                if (thisResult.IsSingle == true)
                {
                    thisResult.ThisObject = thisResult.SimpleFunction();
                    return thisResult.ThisObject;
                }
                return thisResult.SimpleFunction();
            }
            throw new BasicBlankException("I think I need to know what function to run to get the object");
        }
        private object GetInstance(CustomBasicList<ContainerData> thisList, Type thisType) //this is because you already have more than one that matches.
        {
            if (thisList.Count < 2)
                throw new BasicBlankException("It should have been obvious which one it was");
            if (ResolveCategory == EnumResolveCategory.ShowError)
                throw new BasicBlankException($"There was {thisList.Count} Items That Had A Match.  Was Trying To Resolve {thisType.Name} Either Change The Revolve Category Or Rethink");
            thisList.Sort();
            ContainerData firstItem;
            ContainerData secondItem;
            firstItem = thisList[0];
            secondItem = thisList[1];
            if (firstItem.IntendedTypeOnly == false)
            {
                switch (ResolveCategory)
                {

                    case EnumResolveCategory.ManuelPriority:
                        if (firstItem.Priority == secondItem.Priority)
                            throw new BasicBlankException("Priorities can't have duplicates.");
                        break;
                    case EnumResolveCategory.FIFO:

                    case EnumResolveCategory.LIFO:
                        if (firstItem.ID == secondItem.ID)
                            throw new BasicBlankException("Sorting By ID Failed");
                        break;
                    //case EnumResolveCategory.Factory:
                    //    if (firstItem.WasCustomFunction == true & secondItem.WasCustomFunction == true)
                    //        throw new BasicBlankException("I think there should not have been 2 custom functions.  Maybe sorting by custom function failed.");
                    //    if (firstItem.WasCustomFunction == false && thisList.Exists(Items => Items.WasCustomFunction == true))
                    //        throw new BasicBlankException("For sure, sorting by custom function failed because the first item obtained was not a custom function");
                    //    if (firstItem.WasCustomFunction == false)
                    //        throw new BasicBlankException("There was no factories or custom funtions.  Therefore it was not appropriate to use factories category in this case");
                    //    break;
                    default:
                        throw new BasicBlankException("GetInstance Rethink");
                }
            }
            else if (firstItem.IntendedTypeOnly == true && secondItem.IntendedTypeOnly == true)
                throw new BasicBlankException("Most likely error did not get caught because can't have 2 classes that are types only that are the same type.  Otherwise, too complex");
            return GetInstance(firstItem);
        }

        private object GetInstance(Type thisType) //i want to be forced to use generics.
        {

            //if (ParentFactory != null)
            //    return ParentFactory.GetReturnObject(_thisSet.ToCustomBasicList(), thisType); //this means if i have a parent factory, then the parent is responsible for everything.
            //i could change my mind if i choose to.  i'll have to decide later if that was the right decision or not (?)
            CustomBasicList<ContainerData> tempList;
            tempList = _thisSet.Where(items => items.TypeIn == thisType).ToCustomBasicList();
            //bool HadAtLeastOne = false;
            //tempList.RemoveAllOnly(Items => Items.ThisFact != null && Items.ThisFact.CanAcceptObject(tempList, thisType) == false);
            if (tempList.Count == 1)
                return GetInstance(tempList.Single());
            if (tempList.Count > 1 && ResolveCategory == EnumResolveCategory.ManuelPriority)
            {
                //HadAtLeastOne = true;
                tempList.RemoveAllOnly(Items => Items.Priority == 0);
            }
            if (tempList.Count > 1)
                return GetInstance(tempList, thisType);
            tempList = _thisSet.Where(Items => Items.TypeOut == thisType).ToCustomBasicList();
            //tempList.RemoveAllOnly(Items => Items.ThisFact != null && Items.ThisFact.CanAcceptObject(tempList, thisType) == false);
            if (tempList.Count > 1 && ResolveCategory == EnumResolveCategory.ManuelPriority)
            {
                //HadAtLeastOne = true;
                tempList.RemoveAllOnly(Items => Items.Priority == 0);
            }
            if (tempList.Count > 1)
                return GetInstance(tempList, thisType);
            if (tempList.Count == 1)
                return GetInstance(tempList.Single());
            tempList = _thisSet.Where(Items => thisType.IsAssignableFrom(Items.TypeOut)).ToCustomBasicList();
            if (tempList.Count == 1)
                return GetInstance(tempList.Single());
            throw new BasicBlankException($"Looks Like Type {thisType.Name} Was Not Registered.  If I am wrong, rethink");
            //if (tempList.Count == 0 && FactoryList.Count == 0)
            //    throw new BasicBlankException($"Looks Like Type {thisType.Name} Was Not Registered And Had No Factories.  If It Was, Rethink");
            //tempList = _thisSet.ToCustomBasicList();
            //CustomBasicList<IContainerFactory> ResultList = FactoryList.Where(Items => Items.CanAcceptObject(tempList, thisType) == true).ToCustomBasicList();
            //if (ResultList.Count > 1)
            //    throw new BasicBlankException($"There is more than one result found for requested type {thisType.Name}.  I think that rethinking is required now");
            //if (ResultList.Count == 0 && HadAtLeastOne == false)
            //    throw new BasicBlankException($"There was nothing registered with the name {thisType.Name}.  No factories was able to handle this request");
            //else if (ResultList.Count == 0)
            //    throw new BasicBlankException($"Since you set the priority to manuel priority, you are required to manually set the priority to resolve duplicates.  Type Was {thisType.Name}.  Most likely all the ones with the duplicates has to be set manually, choose another option or rethink");
            //return ResultList.Single().GetReturnObject(tempList, thisType);
        }
        private object PrivateInstance(Type thisType)
        {
            var constructor = thisType.GetConstructors().OrderByDescending(Items => Items.GetParameters().Length).FirstOrDefault(); //in the video, its first or default.
            var args = constructor.GetParameters().Select(Items => GetInstance(Items.ParameterType)).ToArray(); //for his test.  i could decide to use my custom collection instead.
            return Activator.CreateInstance(thisType, args);
        }
        public void RegisterInstance<TIn, TOut>(int tPriority = 0, object? thisObject = null) where TOut : TIn
        {
            ContainerData thisResults = new ContainerData()
            {
                IsSingle = false,
                TypeOut = typeof(TOut), //i think
                TypeIn = typeof(TIn),
                SimpleFunction = new Func<object>(() => PrivateInstance(typeof(TOut)))
            };
            SetResults(thisResults, tPriority, thisObject!);
        }
        public void RegisterType<TIn>(bool isSingleton) //i think if you want to register a type, you are not allowed to use a factory.
        {
            //in this case, needs to be kept simple.  otherwise, gets too hard.
            ContainerData thisResults = new ContainerData()
            {
                IsSingle = isSingleton,
                TypeOut = typeof(TIn),
                TypeIn = typeof(TIn),
                IntendedTypeOnly = true,
                SimpleFunction = new Func<object>(() => PrivateInstance(typeof(TIn)))
            };
            SetResults(thisResults, 0, null!);
        }
        //void RegisterSingleton(Type ThisType)
        //{
        //    //looks like another entry here.
        //    ContainerData ThisResults = new ContainerData()
        //    {
        //        TypeOut = ThisType,
        //        TypeIn = ThisType,
        //        IsSingle = true,
        //        IntendedTypeOnly = true, //i think
        //        SimpleFunction = new Func<object>(() => PrivateInstance(ThisType))
        //    };
        //    SetResults(ThisResults, 0, null);
        //}
        public void RegisterInstance<TIn>(Func<TIn> thisFunct, int TPriority = 0, object? thisObject = null) //i do like it this way because you have complete control of what you will actually get back.
        {
            object thisObj = thisFunct;
            Func<object> newFunct = (Func<object>)thisObj;
            ContainerData ThisResults = new ContainerData()
            {
                TypeIn = typeof(TIn), //this means this is all the information you will receive this time.
                TypeOut = typeof(TIn),
                WasCustomFunction = true, //i think this is the only case where it was custom except for factories.
                SimpleFunction = newFunct
            };
            SetResults(ThisResults, TPriority, thisObject);
        }
        //public void RegisterInstance<TIn>(IContainerFactory ThisFact, int TPriority = 0, object ThisObject = null)
        //{
        //    ContainerData ThisResults = new ContainerData()
        //    {
        //        TypeIn = typeof(TIn),
        //        TypeOut = typeof(TIn),
        //        WasCustomFunction = true, //i think this should imply its custom obviously.
        //        ThisFact = ThisFact
        //    };
        //    SetResults(ThisResults, TPriority, ThisObject);
        //}
        public void RegisterStaticVariable<V>(V variable, string tag) where V: IConvertible
        {
            ContainerData ThisResults = new ContainerData()
            {
                IsSingle = true,
                TypeIn = typeof(V),
                TypeOut = typeof(V),
                ThisObject = variable
            };
            SetResults(ThisResults, 0, tag); //i think
        }
        public void RegisterSingleton<TIn>(TIn ourObject, int tPriority = 0, object? thisObject = null)
        {
            if (ourObject == null)
                throw new BasicBlankException("You can't register an object that does not exist.  Most likely, you tried to register it too soon.");
            ContainerData thisResults = new ContainerData()
            {
                IsSingle = true,
                TypeIn = typeof(TIn),
                TypeOut = ourObject.GetType(),
                ThisObject = ourObject
            };
            SetResults(thisResults, tPriority, thisObject);
        }
        private void SetResults(ContainerData thisResults, int tPriority, object? thisObject)
        {
            thisResults.Priority = tPriority; //2 different things.
            thisResults.ExtraDetails = thisObject;
            ID++;
            thisResults.ID = ID; //needed for lifo and fifo methods.
            //ThisResults.Check(); //so if something is wrong, will error out at this moment.
            _thisSet.Add(thisResults);
        }

        public void RegisterSingleton<TIn>(Func<TIn> ThisFunct, int TPriority = 0, object? ThisObject = null) //i do like it this way because you have complete control of what you will actually get back.
        {
            object ThisObj = ThisFunct;
            Func<object> NewFunct = (Func<object>)ThisObj;
            ContainerData ThisResults = new ContainerData()
            {
                IsSingle = true,
                TypeIn = typeof(TIn),
                TypeOut = typeof(TIn), //this means this is all the information you will receive this time.
                SimpleFunction = NewFunct
            };
            SetResults(ThisResults, TPriority, ThisObject);
        }
        //public void RegisterSingleton<TIn>(IContainerFactory ThisFact, int TPriority = 0, object ThisObject = null)
        //{
        //    ContainerData ThisResults = new ContainerData()
        //    {
        //        IsSingle = true,
        //        TypeIn = typeof(TIn),
        //        TypeOut = typeof(TIn),
        //        ThisFact = ThisFact
        //    };
        //    AddFactoryToContainer(ThisFact); //i think i should automatically add a factory for a possibility if i do one.  if i am wrong, can rethink.
        //    SetResults(ThisResults, TPriority, ThisObject);
        //}

        public void RegisterSingleton<TIn, TOut>(int tPriority = 0, object? thisObject = null) where TOut : TIn //if you specified priority on any, then all will be priority.
        {
            ContainerData thisResults = new ContainerData()
            {
                TypeOut = typeof(TOut),
                TypeIn = typeof(TIn),
                IsSingle = true,
                SimpleFunction = new Func<object>(() => PrivateInstance(typeof(TOut)))
            };
            SetResults(thisResults, tPriority, thisObject);
        }
        T IResolver.Resolve<T>()
        {
            return GetInstance<T>();
        }
        public void ReplaceObject<T>(T newObject)
        {
            Type thisType = typeof(T);
            try
            {
                ContainerData thisData = _thisSet.Where(Items => Items.TypeOut == thisType && Items.IsSingle == true).Single();
                thisData.ThisObject = newObject;
            }
            catch(Exception ex)
            {
                throw new BasicBlankException($"Unable to replace object.  The type you were trying to replace is {thisType.Name}.  Error was {ex.Message}");
            }
        }

        

        
    }
}