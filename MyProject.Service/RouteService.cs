using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;

namespace MyProject.Service
{
	public class RouteService : IRouteService
	{
		private readonly IRouteRepository _routeRepository;
		private readonly IUnitOfWork _unitOfWork;

		public RouteService(IRouteRepository routeRepository, IUnitOfWork unitOfWork)
		{
			this._routeRepository = routeRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IRouteService Members

		public IEnumerable<Route> GetRoutes()
		{
			var routes = _routeRepository.GetAll();
			return routes;
		}

		public IEnumerable<Route> GetRoutes(string type)
		{
			var routes = _routeRepository.GetAll(type);
			return routes;
		}
		public Route GetRoute(long id)
		{
			var route = _routeRepository.GetById(id);
			return route;
		}

		public bool CreateRoute(Route route, ref string message)
		{
			try
			{
				if (_routeRepository.GetRouteByName(route.Name) == null)
				{
					_routeRepository.Add(route);
					if (SaveRoute())
					{
						message = "Route added successfully ...";
						return true;
					}
					else
					{
						message = "Oops! Something went wrong. Please try later...";
						return false;
					}
				}
				else
				{
					message = "Route already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool UpdateRoute(ref Route route, ref string message)
		{
			try
			{
				if (_routeRepository.GetRouteByName(route.Name, route.ID) == null)
				{
					Route CurrentRoute = _routeRepository.GetById(route.ID);

					CurrentRoute.Name = route.Name;
					CurrentRoute.Url = route.Url;
					CurrentRoute.Name = route.Type;
					CurrentRoute.ParentID = route.ParentID;
					CurrentRoute.Icon = route.Icon;
					CurrentRoute.Position = route.Position;
					route = null;

					_routeRepository.Update(CurrentRoute);
					if (SaveRoute())
					{
						route = CurrentRoute;
						message = "Route updated successfully ...";
						return true;
					}
					else
					{
						message = "Oops! Something went wrong. Please try later...";
						return false;
					}
				}
				else
				{
					message = "Route already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool DeleteRoute(long id, ref string message)
		{
			try
			{
				Route route = _routeRepository.GetById(id);

				_routeRepository.Delete(route);
				if (SaveRoute())
				{
					message = "Route deleted successfully ...";
					return true;
				}
				else
				{
					message = "Oops! Something went wrong. Please try later...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool SaveRoute()
		{
			try
			{
				_unitOfWork.Commit();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		#endregion
	}

	public interface IRouteService
	{
		IEnumerable<Route> GetRoutes();
		IEnumerable<Route> GetRoutes(string type);
		Route GetRoute(long id);
		bool CreateRoute(Route route, ref string message);
		bool UpdateRoute(ref Route route, ref string message);
		bool DeleteRoute(long id, ref string message);
		bool SaveRoute();
	}
}
