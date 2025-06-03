import styled from 'styled-components';

// Color palette - Updated to use GlobalData design system
export const colors = {
  // GlobalData Primary Colors
  primary: '#2e293d',        // gd-primary
  primaryDark: '#231142',    // gd-primary-dark
  secondary: '#0034ec',      // gd-secondary
  secondaryStandard: '#231142', // gd-secondary-standard
  accent: '#ffa726',
  
  // Background & Surface
  background: '#f5f7fa',     // gd-background - updated to a softer gray-blue
  surface: '#ffffff',       // gd-surface
  surfaceSecondary: '#f0f2f5', // gd-surface-secondary - slightly darker than main background
  
  // Text Colors
  text: '#2e293d',          // gd-text-primary
  textSecondary: '#6c757d', // gd-text-secondary
  textMuted: '#868e96',     // gd-text-muted
  
  // Border & Dividers
  border: '#dee2e6',        // gd-border
  borderLight: '#f1f3f4',   // gd-border-light
  
  // Status Colors
  success: '#28a745',       // gd-success
  error: '#dc3545',         // gd-danger
  warning: '#ffc107',       // gd-warning
  info: '#17a2b8',          // gd-info
};

// Common styled components
export const Container = styled.div`
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 20px;
`;

export const Card = styled.div`
  background: ${colors.surface};
  border-radius: 0.375rem;
  padding: 1.5rem;
  margin-bottom: 1.5rem;
  box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
  border: 1px solid ${colors.borderLight};
  transition: box-shadow 0.15s ease-in-out;
  will-change: transform, box-shadow;
  contain: layout;

  &:hover {
    box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
    transform: translateZ(0);
  }
`;

export const Button = styled.button<{ variant?: 'primary' | 'secondary' | 'outline' | 'success' | 'danger' }>`
  padding: 0.5rem 1rem;
  border-radius: 0.25rem;
  border: 1px solid transparent;
  cursor: pointer;
  font-size: 0.875rem;
  font-weight: 500;
  line-height: 1.5;
  transition: all 0.15s ease-in-out;
  text-decoration: none;
  display: inline-block;
  text-align: center;
  font-family: 'IBM Plex Sans', sans-serif;

  ${props => {
    switch (props.variant) {
      case 'secondary':
        return `
          background: ${colors.secondary};
          color: white;
          border-color: ${colors.secondary};
          &:hover {
            background: ${colors.secondaryStandard};
            border-color: ${colors.secondaryStandard};
          }
        `;
      case 'outline':
        return `
          background: transparent;
          color: ${colors.primary};
          border-color: ${colors.primary};
          &:hover {
            background: ${colors.primary};
            color: white;
            border-color: ${colors.primary};
          }
        `;
      case 'success':
        return `
          background: ${colors.success};
          color: white;
          border-color: ${colors.success};
          &:hover {
            background: #218838;
            border-color: #1e7e34;
          }
        `;
      case 'danger':
        return `
          background: ${colors.error};
          color: white;
          border-color: ${colors.error};
          &:hover {
            background: #c82333;
            border-color: #bd2130;
          }
        `;
      default:
        return `
          background: ${colors.primary};
          color: white;
          border-color: ${colors.primary};
          &:hover {
            background: ${colors.primaryDark};
            border-color: ${colors.primaryDark};
          }
        `;
    }
  }}

  &:focus {
    outline: 0;
    box-shadow: 0 0 0 0.2rem rgba(46, 41, 61, 0.25);
  }

  &:disabled {
    opacity: 0.65;
    cursor: not-allowed;
  }
`;

export const Input = styled.input`
  padding: 0.5rem 0.75rem;
  border: 1px solid ${colors.border};
  border-radius: 0.25rem;
  font-size: 0.875rem;
  line-height: 1.5;
  width: 100%;
  box-sizing: border-box;
  font-family: 'IBM Plex Sans', sans-serif;
  background-color: ${colors.surface};
  color: ${colors.text};
  transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;

  &:focus {
    outline: none;
    border-color: #80bdff;
    box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
  }

  &::placeholder {
    color: ${colors.textMuted};
  }
`;

export const Select = styled.select`
  padding: 0.5rem 0.75rem;
  border: 1px solid ${colors.border};
  border-radius: 0.25rem;
  font-size: 0.875rem;
  line-height: 1.5;
  width: 100%;
  box-sizing: border-box;
  font-family: 'IBM Plex Sans', sans-serif;
  background-color: ${colors.surface};
  color: ${colors.text};
  transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;

  &:focus {
    outline: none;
    border-color: #80bdff;
    box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
  }
`;

export const Grid = styled.div<{ columns?: number; gap?: string }>`
  display: grid;
  grid-template-columns: repeat(${props => props.columns || 1}, 1fr);
  gap: ${props => props.gap || '20px'};
  will-change: transform;
  contain: layout;
  
  @media (max-width: 768px) {
    grid-template-columns: 1fr;
  }
`;

export const FlexContainer = styled.div<{ 
  direction?: 'row' | 'column';
  justify?: string;
  align?: string;
  gap?: string;
}>`
  display: flex;
  flex-direction: ${props => props.direction || 'row'};
  justify-content: ${props => props.justify || 'flex-start'};
  align-items: ${props => props.align || 'stretch'};
  gap: ${props => props.gap || '0'};
`;

export const Title = styled.h1`
  color: ${colors.text};
  margin: 0 0 1.5rem 0;
  font-size: 2rem;
  font-weight: 600;
  line-height: 1.2;
  font-family: 'IBM Plex Sans', sans-serif;
`;

export const Subtitle = styled.h2`
  color: ${colors.text};
  margin: 0 0 1rem 0;
  font-size: 1.5rem;
  font-weight: 500;
  line-height: 1.2;
  font-family: 'IBM Plex Sans', sans-serif;
`;

export const Text = styled.p`
  color: ${colors.text};
  line-height: 1.6;
  margin: 0 0 1rem 0;
  font-family: 'IBM Plex Sans', sans-serif;
`;

export const SecondaryText = styled.span`
  color: ${colors.textSecondary};
  font-size: 12px;
`;

export const Badge = styled.span<{ variant?: 'success' | 'error' | 'warning' | 'info' }>`
  padding: 4px 8px;
  border-radius: 12px;
  font-size: 11px;
  font-weight: 500;
  text-transform: uppercase;

  ${props => {
    switch (props.variant) {
      case 'success':
        return `background: ${colors.success}; color: white;`;
      case 'error':
        return `background: ${colors.error}; color: white;`;
      case 'warning':
        return `background: ${colors.warning}; color: white;`;
      default:
        return `background: ${colors.primary}; color: white;`;
    }
  }}
`;

export const LoadingSpinner = styled.div`
  border: 3px solid ${colors.border};
  border-top: 3px solid ${colors.primary};
  border-radius: 50%;
  width: 30px;
  height: 30px;
  animation: spin 1s linear infinite;
  margin: 20px auto;
  will-change: transform;

  @keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
  }
`;

export const ErrorMessage = styled.div`
  background: #ffebee;
  color: ${colors.error};
  padding: 15px;
  border-radius: 4px;
  border-left: 4px solid ${colors.error};
  margin: 10px 0;
`;

export const SuccessMessage = styled.div`
  background: #e8f5e8;
  color: ${colors.success};
  padding: 15px;
  border-radius: 4px;
  border-left: 4px solid ${colors.success};
  margin: 10px 0;
`;

// Page-specific components
export const PageContainer = styled.div`
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
`;

export const PageTitle = styled.h1`
  color: ${colors.text};
  margin: 0 0 30px 0;
  font-size: 32px;
  font-weight: 600;
  text-align: center;
`;

export const FilterContainer = styled.div`
  background: ${colors.surface};
  border-radius: 8px;
  padding: 20px;
  margin-bottom: 30px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  border: 1px solid ${colors.border};
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 15px;
  align-items: end;
`;

export const FilterGroup = styled.div`
  display: flex;
  flex-direction: column;
  gap: 5px;
`;

export const FilterLabel = styled.label`
  color: ${colors.text};
  font-size: 14px;
  font-weight: 500;
`;

export const FilterInput = styled(Input)``;

export const FilterSelect = styled(Select)``;

export const FilterButton = styled(Button)`
  white-space: nowrap;
`;

export const GridContainer = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(400px, 1fr));
  gap: 20px;
  margin-bottom: 30px;
`;

export const NoResultsMessage = styled.div`
  text-align: center;
  color: ${colors.textSecondary};
  font-size: 16px;
  padding: 40px 20px;
  background: ${colors.surface};
  border-radius: 8px;
  border: 1px solid ${colors.border};
`;
