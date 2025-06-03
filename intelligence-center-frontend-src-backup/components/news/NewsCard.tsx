import React from 'react';
import styled from 'styled-components';
import { NewsItem } from '../../types/api';
import { Card, Subtitle, Text, SecondaryText, Badge } from '../styled/StyledComponents';

const NewsItemContainer = styled(Card)`
  transition: transform 0.2s ease, box-shadow 0.2s ease;
  cursor: pointer;

  &:hover {
    transform: translateY(-2px);
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.15);
  }
`;

const NewsHeader = styled.div`
  margin-bottom: 10px;
`;

const CompanyTags = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: 5px;
  margin-top: 10px;
`;

const DateText = styled(SecondaryText)`
  display: block;
  margin-bottom: 10px;
`;

interface NewsCardProps {
  news: NewsItem;
  onClick?: () => void;
}

const NewsCard: React.FC<NewsCardProps> = ({ news, onClick }) => {
  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  };

  const truncateText = (text: string, maxLength: number) => {
    if (text.length <= maxLength) return text;
    return text.substring(0, maxLength) + '...';
  };

  return (    <NewsItemContainer onClick={onClick}>
      <NewsHeader>
        <DateText>{formatDate(news.publishedDate)}</DateText>
        <Subtitle>{news.title}</Subtitle>
      </NewsHeader>
      
      {news.summary && (
        <Text>{truncateText(news.summary, 200)}</Text>
      )}
      
      {news.companies && news.companies.length > 0 && (
        <CompanyTags>
          {news.companies.slice(0, 3).map((company, index) => (
            <Badge key={company.companyId || index} variant="info">
              {company.companyName}
            </Badge>
          ))}
          {news.companies.length > 3 && (
            <Badge variant="info">+{news.companies.length - 3} more</Badge>
          )}
        </CompanyTags>
      )}
      
      {news.relatedCompanyNames && news.relatedCompanyNames.length > 0 && !news.companies && (
        <CompanyTags>
          {news.relatedCompanyNames.slice(0, 3).map((companyName, index) => (
            <Badge key={index} variant="info">
              {companyName}
            </Badge>
          ))}
          {news.relatedCompanyNames.length > 3 && (
            <Badge variant="info">+{news.relatedCompanyNames.length - 3} more</Badge>
          )}
        </CompanyTags>
      )}
    </NewsItemContainer>
  );
};

export default NewsCard;
