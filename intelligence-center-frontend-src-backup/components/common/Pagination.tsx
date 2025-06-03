import React from 'react';
import styled from 'styled-components';
import { Button, FlexContainer, Text } from '../styled/StyledComponents';

const PaginationContainer = styled(FlexContainer)`
  justify-content: space-between;
  align-items: center;
  margin: 20px 0;
  flex-wrap: wrap;
  gap: 10px;
`;

const PaginationButtons = styled(FlexContainer)`
  gap: 10px;
`;

const PageInfo = styled(Text)`
  color: #666;
  font-size: 14px;
`;

const PageSizeSelector = styled.select`
  padding: 5px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
`;

interface PaginationProps {
  currentPage: number;
  totalPages: number;
  totalItems: number;
  pageSize: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  onPageChange: (page: number) => void;
  onPageSizeChange?: (pageSize: number) => void;
}

const Pagination: React.FC<PaginationProps> = ({
  currentPage,
  totalPages,
  totalItems,
  pageSize,
  hasNextPage,
  hasPreviousPage,
  onPageChange,
  onPageSizeChange
}) => {
  const startItem = (currentPage - 1) * pageSize + 1;
  const endItem = Math.min(currentPage * pageSize, totalItems);

  const handleFirstPage = () => onPageChange(1);
  const handlePrevPage = () => onPageChange(currentPage - 1);
  const handleNextPage = () => onPageChange(currentPage + 1);
  const handleLastPage = () => onPageChange(totalPages);

  return (
    <PaginationContainer>
      <FlexContainer gap="10px" align="center">
        <PageInfo>
          Showing {startItem}-{endItem} of {totalItems.toLocaleString()} items
        </PageInfo>
        {onPageSizeChange && (
          <FlexContainer gap="5px" align="center">
            <PageInfo>Show:</PageInfo>
            <PageSizeSelector
              value={pageSize}
              onChange={(e) => onPageSizeChange(Number(e.target.value))}
            >
              <option value={10}>10</option>
              <option value={25}>25</option>
              <option value={50}>50</option>
              <option value={100}>100</option>
            </PageSizeSelector>
          </FlexContainer>
        )}
      </FlexContainer>
      
      <PaginationButtons>
        <Button
          variant="outline"
          onClick={handleFirstPage}
          disabled={!hasPreviousPage}
        >
          First
        </Button>
        
        <Button
          variant="outline"
          onClick={handlePrevPage}
          disabled={!hasPreviousPage}
        >
          Previous
        </Button>
        
        <PageInfo style={{ margin: '0 10px' }}>
          Page {currentPage} of {totalPages}
        </PageInfo>
        
        <Button
          variant="outline"
          onClick={handleNextPage}
          disabled={!hasNextPage}
        >
          Next
        </Button>
        
        <Button
          variant="outline"
          onClick={handleLastPage}
          disabled={!hasNextPage}
        >
          Last
        </Button>
      </PaginationButtons>
    </PaginationContainer>
  );
};

export default Pagination;
