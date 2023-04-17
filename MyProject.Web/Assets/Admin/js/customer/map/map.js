var latitude;
var longitude;
var IsMapLoaded = false;
var MapAddress;
var IsGetCurrentLocation = false;
var DefaultMapMarker = "/Assets/images/web-icons/map-marker.png";

$(document).ready(function () {
	$('.get-current-location').click(function (e) {
		getLocation();
	});
	openMap_1();

	if (IsInitAutoCompleteRun) {
		initAutocomplete();
		getLocation();
	}
});

function SetCurrentLocation(lat, lon, adr) {
	var currentLocation = {
		latitude: lat,
		longitude: lon,
		address: adr,
	}
	localStorage.setItem("currentLocation", JSON.stringify(currentLocation));
}

function getLocation() {
	if (navigator.geolocation) {
		navigator.geolocation.getCurrentPosition(showPosition);
		IsGetCurrentLocation = true;
	} else {
		alert("Geolocation is not supported by this browser.");

		latitude = 25.2048
		longitude = 55.2708
		InsertPosition(latitude, longitude);
		InsertLatLonInInput(latitude, longitude);
	}
}

function showPosition(position) {

	latitude = position.coords.latitude;
	longitude = position.coords.longitude;

	InsertLatLonInInput(latitude, longitude);
	InsertPosition(latitude, longitude);

}

function InsertPosition(latitude, longitude) {
	$.ajax({
		type: 'Get',
		url: `https://maps.googleapis.com/maps/api/geocode/json?latlng=${latitude},${longitude}&key=AIzaSyD96pSIJVVnH9RDyy-n2G3uu3FG8Ze9xyc`,
		success: function (response) {
			if (response.results.length > 0) {

				MapAddress = response.results[0].formatted_address;

				InsertMapAddressInInput(MapAddress);

				if (IsGetCurrentLocation) {
					SetCurrentLocation(latitude, longitude, MapAddress);
					IsGetCurrentLocation = false;
				}

			} else {
				alert("Geolocation is not supported by this browser.");
			}
		}
	});
}

function InsertLatLonInInput(latitude, longitude) {
	$('input[name="Latitude"]').val(latitude);
	$('input[name="Longitude"]').val(longitude);
}

function InsertMapAddressInInput(MapAddress) {
	//$('#Address').val(MapAddress);
	$('input[name="MapLocation"]').val(MapAddress);
}


function myMap_1() {

	latitude = latitude ? latitude : 25.2048;
	longitude = longitude ? longitude : 25.2048;

	var map = new google.maps.Map(document.getElementById('google_map'), {
		zoom: 15,
		center: new google.maps.LatLng(latitude, longitude),
		mapTypeId: google.maps.MapTypeId.ROADMAP
	});

	//var mImage = new google.maps.MarkerImage(DefaultMapMarker,
	//	new google.maps.Size(34, 35),
	//	new google.maps.Point(0, 10),
	//	new google.maps.Point(10, 34)
	//);

	var myMarker = new google.maps.Marker({
		position: new google.maps.LatLng(latitude, longitude),
		draggable: true,
		icon: DefaultMapMarker,
	});

	google.maps.event.addListener(myMarker, 'dragend', function (evt) {
		document.getElementById('drag-map').innerHTML = '<span>' + ChangeString('Drag marker on the map to select your desired location.', 'اسحب علامة على الخريطة لتحديد الموقع المطلوب.') + '</span>';

		latitude = evt.latLng.lat().toFixed(3);
		longitude = evt.latLng.lng().toFixed(3);

		InsertLatLonInInput(latitude, longitude);
		InsertPosition(latitude, longitude);

	});
	google.maps.event.addListener(myMarker, 'dragstart', function (evt) {
		document.getElementById('drag-map').innerHTML = '<span>' + ChangeString('Currently dragging marker ...', 'جارٍ سحب العلامة حاليًا ...') + '</span>';
	});
	map.setCenter(myMarker.position);
	myMarker.setMap(map);

}

function initAutocomplete() {
	IsInitAutoCompleteRun = true;

	var defaultBounds = new google.maps.LatLngBounds();
	var options = {
		bounds: defaultBounds
	};

	var inputs = document.getElementsByClassName('search-location');

	var autocompletes = [];

	for (var i = 0; i < inputs.length; i++) {
		var autocomplete = new google.maps.places.Autocomplete(inputs[i], options);
		autocomplete.inputId = inputs[i].id;
		autocomplete.addListener('place_changed', fillIn);
		autocompletes.push(autocomplete);
	}

	function fillIn() {

		var place = this.getPlace();


		latitude = place.geometry.location.lat();
		longitude = place.geometry.location.lng();

		InsertLatLonInInput(latitude, longitude);

		myMap_1();

		MapAddress = place.formatted_address;

		InsertMapAddressInInput(MapAddress);

	}

	myMap_1();
}

function openMap_1() {
	if (!IsMapLoaded) {

		if (!latitude) {
			getLocation();
		}
		setTimeout(function () {
			myMap_1();
			$('.map-div-spin').hide();
			$('#google_map').show();
		}, 1000)
		IsMapLoaded = true;
	}
	else {
		myMap_1();
		InsertMapAddressInInput(MapAddress)
	}
	//$('#map-modal').modal('show');
}
