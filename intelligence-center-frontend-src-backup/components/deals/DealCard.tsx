import React from 'react';
import styled from 'styled-components';
import { DealItem } from '../../types/api';
import { Card, Subtitle, SecondaryText, Badge } from '../styled/StyledComponents';

const DealItemContainer = styled(Card)`
  transition: transform 0.2s ease, box-shadow 0.2s ease;
  cursor: pointer;

  &:hover {
    transform: translateY(-2px);
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.15);
  }
`;

const DealHeader = styled.div`
  margin-bottom: 15px;
`;

const DealValue = styled.div`
  font-size: 18px;
  font-weight: 600;
  color: #2e7d32;
  margin: 10px 0;
`;

const DealMeta = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
  margin-top: 15px;
`;

const DateText = styled(SecondaryText)`
  display: block;
  margin-bottom: 10px;
`;

interface DealCardProps {
  deal: DealItem;
  onClick?: () => void;
}

const DealCard: React.FC<DealCardProps> = ({ deal, onClick }) => {
  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  };

  const formatCurrency = (value: number) => {
    if (value >= 1000000000) {
      return `$${(value / 1000000000).toFixed(1)}B`;
    } else if (value >= 1000000) {
      return `$${(value / 1000000).toFixed(1)}M`;
    } else if (value >= 1000) {
      return `$${(value / 1000).toFixed(1)}K`;
    }
    return `$${value.toLocaleString()}`;
  };

  const getStatusVariant = (status: string) => {
    switch (status.toLowerCase()) {
      case 'completed':
        return 'success';
      case 'pending':
        return 'warning';
      case 'cancelled':
        return 'error';
      default:
        return 'info';
    }
  };

  const getDealTypeVariant = (dealType: string) => {
    switch (dealType.toLowerCase()) {
      case 'merger':
        return 'info';
      case 'acquisition':
        return 'success';
      default:
        return 'info';
    }
  };

  return (
    <DealItemContainer onClick={onClick}>
      <DealHeader>
        <DateText>{formatDate(deal.publishedDate)}</DateText>
        <Subtitle>{deal.title}</Subtitle>
      </DealHeader>

      {deal.dealValue > 0 && (
        <DealValue>{formatCurrency(deal.dealValue)}</DealValue>
      )}

      <DealMeta>
        {deal.dealType && (
          <Badge variant={getDealTypeVariant(deal.dealType)}>
            {deal.dealType}
          </Badge>
        )}
        
        {deal.dealStatus && (
          <Badge variant={getStatusVariant(deal.dealStatus)}>
            {deal.dealStatus}
          </Badge>
        )}
        
        {deal.dealCountryvalue && (
          <Badge variant="info">
            {deal.dealCountryvalue}
          </Badge>
        )}
      </DealMeta>
    </DealItemContainer>
  );
};

export default DealCard;
