import React, { useEffect, useRef, memo, useMemo, useCallback } from 'react';
import { MapContainer, TileLayer, Marker, Popup, useMap } from 'react-leaflet';
import { Icon, LatLngBounds } from 'leaflet';
import styled from 'styled-components';
import { CountryAggregation } from '../../types/dashboard';
import { Card } from '../styled/StyledComponents';
import { dashboardService } from '../../services/dashboardService';

// Fix Leaflet default icon issue with webpack
import markerIcon2x from 'leaflet/dist/images/marker-icon-2x.png';
import markerIcon from 'leaflet/dist/images/marker-icon.png';
import markerShadow from 'leaflet/dist/images/marker-shadow.png';

// Configure default marker icon
delete (Icon.Default.prototype as any)._getIconUrl;
Icon.Default.mergeOptions({
  iconUrl: markerIcon,
  iconRetinaUrl: markerIcon2x,
  shadowUrl: markerShadow,
});

const MapWrapper = styled(Card)`
  height: 500px;
  padding: 0;
  overflow: hidden;
  position: relative;
`;

const MapLegend = styled.div`
  position: absolute;
  bottom: 15px;
  right: 15px;
  background: rgba(255, 255, 255, 0.95);
  padding: 12px;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
  z-index: 1000;
  font-size: 12px;
  min-width: 200px;
`;

const LegendTitle = styled.div`
  font-weight: 600;
  margin-bottom: 8px;
  color: #2e293d;
  font-size: 13px;
`;

const LegendItem = styled.div`
  display: flex;
  justify-content: space-between;
  margin: 4px 0;
  color: #666;
`;

const MarkerSizeIndicator = styled.div`
  display: flex;
  align-items: center;
  gap: 8px;
  margin-top: 8px;
  padding-top: 8px;
  border-top: 1px solid #e0e0e0;
`;

const SampleMarker = styled.div<{ size: number }>`
  width: ${props => props.size}px;
  height: ${props => props.size}px;
  background: #2e293d;
  border-radius: 50%;
  opacity: 0.7;
`;

const PopupContent = styled.div`
  min-width: 200px;
`;

const PopupTitle = styled.h4`
  margin: 0 0 8px 0;
  font-size: 16px;
  font-weight: 600;
  color: #2e293d;
`;

const PopupStats = styled.div`
  display: flex;
  flex-direction: column;
  gap: 4px;
`;

const StatItem = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 14px;
`;

const StatLabel = styled.span`
  color: #666;
`;

const StatValue = styled.span`
  font-weight: 600;
  color: #2e293d;
`;

interface DealsWorldMapProps {
  countries: CountryAggregation[];
  loading?: boolean;
}

// Custom marker icon based on deal volume
const createCustomIcon = (totalDeals: number): Icon => {
  let size: [number, number] = [25, 41];
  let iconUrl = markerIcon;

  // Scale marker size based on deal volume
  if (totalDeals >= 5) {
    size = [35, 57];
  } else if (totalDeals >= 3) {
    size = [30, 49];
  }

  return new Icon({
    iconUrl,
    iconRetinaUrl: markerIcon2x,
    shadowUrl: markerShadow,
    iconSize: size,
    iconAnchor: [size[0] / 2, size[1]],
    popupAnchor: [1, -34],
    shadowSize: [41, 41]
  });
};

// Component to fit bounds when countries change - memoized for performance
const FitBounds: React.FC<{ countries: CountryAggregation[] }> = memo(({ countries }) => {
  const map = useMap();

  useEffect(() => {
    if (countries.length > 0) {
      const bounds = new LatLngBounds(
        countries.map(country => [country.latitude, country.longitude])
      );
      map.fitBounds(bounds, { padding: [20, 20] });
    }
  }, [countries, map]);

  return null;
});

// Memoized marker component for better performance
const DealMarker: React.FC<{ country: CountryAggregation }> = memo(({ country }) => {
  const formatCurrency = useCallback((value: number): string => {
    return dashboardService.formatLargeNumber(value);
  }, []);

  const customIcon = useMemo(() => createCustomIcon(country.totalDeals), [country.totalDeals]);

  const avgDealSize = useMemo(() => 
    formatCurrency(country.totalValue / country.totalDeals), 
    [country.totalValue, country.totalDeals, formatCurrency]
  );

  return (
    <Marker
      position={[country.latitude, country.longitude]}
      icon={customIcon}
    >
      <Popup>
        <PopupContent>
          <PopupTitle>{country.country}</PopupTitle>          <PopupStats>
            <StatItem>
              <StatLabel>Deals Volume:</StatLabel>
              <StatValue>{country.totalDeals}</StatValue>
            </StatItem>
            <StatItem>
              <StatLabel>Value:</StatLabel>
              <StatValue>${formatCurrency(country.totalValue)}</StatValue>
            </StatItem>
            <StatItem>
              <StatLabel>Avg Deal Size:</StatLabel>
              <StatValue>${avgDealSize}</StatValue>
            </StatItem>
          </PopupStats>
        </PopupContent>
      </Popup>
    </Marker>
  );
});

const DealsWorldMap: React.FC<DealsWorldMapProps> = memo(({ countries, loading = false }) => {
  const mapRef = useRef<any>(null);

  // Calculate totals for legend
  const totalDeals = useMemo(() => 
    countries.reduce((sum, country) => sum + country.totalDeals, 0), 
    [countries]
  );
  
  const totalValue = useMemo(() => 
    countries.reduce((sum, country) => sum + country.totalValue, 0), 
    [countries]
  );

  const formatCurrency = useCallback((value: number): string => {
    return dashboardService.formatLargeNumber(value);
  }, []);
  if (loading) {
    return (
      <MapWrapper>
        <div style={{ 
          display: 'flex', 
          justifyContent: 'center', 
          alignItems: 'center', 
          height: '100%',
          color: '#666'
        }}>
          Loading map data...
        </div>
      </MapWrapper>
    );
  }
  return (
    <MapWrapper>
      <MapContainer
        ref={mapRef}
        center={[20, 0]}
        zoom={2}
        style={{ height: '100%', width: '100%' }}
        scrollWheelZoom={true}
        attributionControl={true}
        preferCanvas={true} // Use canvas for better performance
        zoomControl={true}
        doubleClickZoom={true}
        closePopupOnClick={true}
        dragging={true}
        zoomSnap={1}
        zoomDelta={1}
        trackResize={true}
        touchZoom={true}
        bounceAtZoomLimits={true}
      >
        <TileLayer
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
          attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
          maxZoom={18}
          tileSize={256}
          zoomOffset={0}
          updateWhenIdle={true}
          updateWhenZooming={false}
          keepBuffer={2}
        />
        
        {countries.map((country) => (
          <DealMarker key={country.countryCode} country={country} />
        ))}
        
        <FitBounds countries={countries} />
      </MapContainer>      <MapLegend>
        <LegendTitle>Map Overview</LegendTitle>
        <LegendItem>
          <span>Total Countries:</span>
          <span>{countries.length}</span>
        </LegendItem>
        <LegendItem>
          <span>Total Deals Volume:</span>
          <span>{totalDeals}</span>
        </LegendItem>
        <LegendItem>
          <span>Total Value:</span>
          <span>${formatCurrency(totalValue)}</span>
        </LegendItem>
        
        <MarkerSizeIndicator>
          <LegendTitle style={{ margin: 0, fontSize: '12px' }}>Marker Size by Deal Count:</LegendTitle>
        </MarkerSizeIndicator>
        <LegendItem>
          <span>1-2 deals</span>
          <SampleMarker size={20} />
        </LegendItem>
        <LegendItem>
          <span>3-4 deals</span>
          <SampleMarker size={30} />
        </LegendItem>
        <LegendItem>
          <span>5+ deals</span>
          <SampleMarker size={40} />
        </LegendItem>
      </MapLegend>
    </MapWrapper>
  );
});

export default DealsWorldMap;
