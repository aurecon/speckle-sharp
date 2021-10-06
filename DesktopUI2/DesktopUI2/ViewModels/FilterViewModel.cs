﻿using Avalonia.Controls;
using Avalonia.Controls.Selection;
using DesktopUI2.Models.Filters;
using DesktopUI2.Views.Filters;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesktopUI2.ViewModels
{
  public class FilterViewModel : ReactiveObject
  {
    private ConnectorBindings Bindings;

    private ISelectionFilter _filter;
    
    public ISelectionFilter Filter { get=>_filter;
      set
      {
        this.RaiseAndSetIfChanged(ref _filter, value);
        RestoreSelectedItems();
        this.RaisePropertyChanged("Summary");
        
      } 
    }

    public UserControl FilterView { get; private set; }

    public SelectionModel<string> SelectionModel { get; }

    public string Summary { get { return Filter.Summary; } }




    public FilterViewModel(ISelectionFilter filter)
    {
      SelectionModel = new SelectionModel<string>();
      SelectionModel.SingleSelect = false;
      SelectionModel.SelectionChanged += SelectionChanged;

      //use dependency injection to get bindings
      Bindings = Locator.Current.GetService<ConnectorBindings>();

      Filter = filter;
      FilterView = filter.View;

      //TODO should clean up this logic a bit
      //maybe have a model, view and viewmodel for each filter
      if (filter is ListSelectionFilter l)
        _valuesList = SearchResults = new List<string>(l.Values);


      FilterView.DataContext = this;

    }

    #region LIST FILTER

    void SelectionChanged(object sender, SelectionModelSelectionChangedEventArgs e)
    {
      if (!isSearching)
      {
        foreach (var a in e.SelectedItems)
          if (!Filter.Selection.Contains(a))
            Filter.Selection.Add(a as string);
        foreach (var r in e.DeselectedItems)
          Filter.Selection.Remove(r as string);

        this.RaisePropertyChanged("Summary");
      }
    }
    private bool isSearching = false;
    private string _searchQuery;
    public string SearchQuery
    {
      get => _searchQuery;
      set
      {
        isSearching = true;
        this.RaiseAndSetIfChanged(ref _searchQuery, value);

        SearchResults = new List<string>(_valuesList.Where(v => v.ToLower().Contains(SearchQuery.ToLower())).ToList());
        this.RaisePropertyChanged(nameof(SearchResults));
        isSearching = false;
        RestoreSelectedItems();

      }
    }

    // searching will change data source and remove selected items in the ListBox, 
    // restore them as the query is cleared
    public void RestoreSelectedItems()
    {
      foreach (var item in Filter.Selection)
      {
        if (!SelectionModel.SelectedItems.Contains(item))
          SelectionModel.Select(SearchResults.IndexOf(item));
      }
    }

    public List<string> SearchResults { get; set; } = new List<string>();
    private List<string> _valuesList { get; }

    #endregion

    #region MANUAL FILTER
    public void SetObjectSelection()
    {
      var objIds = Bindings.GetSelectedObjects();
      if (objIds == null || objIds.Count == 0)
      {
        //Globals.Notify("Could not get object selection.");
        return;
      }

      Filter.Selection = objIds;
      this.RaisePropertyChanged("Summary");
      //Globals.Notify("Object selection set.");
    }

    public void AddObjectSelection()
    {
      var objIds = Bindings.GetSelectedObjects();
      if (objIds == null || objIds.Count == 0)
      {
        //Globals.Notify("Could not get object selection.");
        return;
      }

      objIds.ForEach(id =>
      {
        if (Filter.Selection.FirstOrDefault(x => x == id) == null)
        {
          Filter.Selection.Add(id);
        }
      });
      this.RaisePropertyChanged("Summary");
      //Globals.Notify("Object added.");

    }

    public void RemoveObjectSelection()
    {
      var objIds = Bindings.GetSelectedObjects();
      if (objIds == null || objIds.Count == 0)
      {
        //Globals.Notify("Could not get object selection.");
        return;
      }

      var filtered = Filter.Selection.Where(o => objIds.IndexOf(o) == -1).ToList();

      if (filtered.Count == Filter.Selection.Count)
      {
        //Globals.Notify("No objects removed.");
        return;
      }
      
      //Globals.Notify($"{Filter.Selection.Count - filtered.Count} objects removed.");
      Filter.Selection = filtered;
      this.RaisePropertyChanged("Summary");
    }

    public void ClearObjectSelection()
    {
      Filter.Selection = new List<string>();
      this.RaisePropertyChanged("Summary");
      //Globals.Notify($"Selection cleared.");
    }
    #endregion

    public bool IsReady()
    {
      if (Filter is ManualSelectionFilter && !Filter.Selection.Any())
        return false;

      if (Filter is ListSelectionFilter && !Filter.Selection.Any())
        return false;

      if (Filter is PropertySelectionFilter p)
      {
        if (string.IsNullOrEmpty(p.PropertyName) || string.IsNullOrEmpty(p.PropertyOperator) || string.IsNullOrEmpty(p.PropertyValue))
          return false;
      }



      return true;

    }

  }
}